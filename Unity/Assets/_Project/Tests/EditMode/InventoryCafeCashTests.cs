using Legacy.Commands;
using Legacy.History;
using Legacy.Save;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class InventoryCafeCashTests
    {
        [Test]
        public void ItemCatalog_ExposesCafeIngredientsAndPreparedGoods()
        {
            Assert.That(ItemCatalog.TryGet(ItemCatalog.CoffeeBeans, out ItemDefinition beans), Is.True);
            Assert.That(ItemCatalog.TryGet(ItemCatalog.PreparedCoffee, out ItemDefinition coffee), Is.True);

            Assert.That(beans.Kind, Is.EqualTo(ItemKind.Ingredient));
            Assert.That(coffee.Kind, Is.EqualTo(ItemKind.PreparedGood));
            Assert.That(coffee.BaseValueCents, Is.EqualTo(275));
        }

        [Test]
        public void CafeRecipe_PrepareConsumesIngredientsAndProducesCoffee()
        {
            CafeRecipeCatalog.TryGet(CafeRecipeCatalog.HouseCoffee, out CafeRecipeDefinition recipe);
            var inventory = new InventoryContainerState(
                new WorldEntityId("inv_test"),
                new WorldEntityId("workplace_test"),
                InventoryContainerKind.WorkplaceStorage,
                "Test storage");
            inventory.TryAdd(ItemCatalog.CoffeeBeans, 2);
            inventory.TryAdd(ItemCatalog.Milk, 2);
            inventory.TryAdd(ItemCatalog.PaperCup, 2);

            bool prepared = CafeRecipeSystem.TryPrepare(inventory, recipe, out string reason);

            Assert.That(prepared, Is.True, reason);
            Assert.That(inventory.CountOf(ItemCatalog.CoffeeBeans), Is.EqualTo(1));
            Assert.That(inventory.CountOf(ItemCatalog.Milk), Is.EqualTo(1));
            Assert.That(inventory.CountOf(ItemCatalog.PaperCup), Is.EqualTo(1));
            Assert.That(inventory.CountOf(ItemCatalog.PreparedCoffee), Is.EqualTo(1));
        }

        [Test]
        public void CashDrawer_AcceptsTenderAndMakesExactChange()
        {
            var drawer = new CashDrawerState(new WorldEntityId("cash_test"), new WorldEntityId("workplace_test"), CashContainerKind.Till, "Test till");
            drawer.Add(CashDenomination.OneDollar, 5);
            drawer.Add(CashDenomination.Quarter, 8);
            int startingTotal = drawer.TotalCents;

            bool accepted = CashDrawerSystem.TryAcceptSale(
                drawer,
                new[] { new CashDenominationStackState(CashDenomination.FiveDollar, 1) },
                275,
                out CashChangeResult change);

            Assert.That(accepted, Is.True, change.Message);
            Assert.That(change.ChangeCents, Is.EqualTo(225));
            Assert.That(drawer.TotalCents, Is.EqualTo(startingTotal + 275));
        }

        [Test]
        public void ProcessCafeSale_UsesRecipeInventoryCashDrawerAndAccounts()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workplaceId = new WorldEntityId("workplace_linden_cafe");
            var customerAccountId = new WorldEntityId("account_holland_cash");
            var cafeAccountId = new WorldEntityId("account_linden_cafe");
            runtime.State.TryGetInventoryContainerForOwner(workplaceId, InventoryContainerKind.WorkplaceStorage, out InventoryContainerState inventory);
            runtime.State.TryGetCashDrawerForOwner(workplaceId, CashContainerKind.Till, out CashDrawerState till);
            int startingCustomerBalance = runtime.State.MoneyAccountsById[customerAccountId].BalanceCents;
            int startingCafeBalance = runtime.State.MoneyAccountsById[cafeAccountId].BalanceCents;
            int startingTillTotal = till.TotalCents;

            WorldCommandResult result = runtime.Execute(new ProcessCafeSaleCommand(
                new WorldEntityId("citizen_rowan"),
                new WorldEntityId("citizen_mr_holland"),
                workplaceId,
                CafeRecipeCatalog.HouseCoffee,
                new[] { new CashDenominationStackState(CashDenomination.FiveDollar, 1) }));

            Assert.That(result.Succeeded, Is.True, result.Message);
            Assert.That(inventory.CountOf(ItemCatalog.CoffeeBeans), Is.EqualTo(24));
            Assert.That(inventory.CountOf(ItemCatalog.Milk), Is.EqualTo(11));
            Assert.That(inventory.CountOf(ItemCatalog.PaperCup), Is.EqualTo(29));
            Assert.That(inventory.CountOf(ItemCatalog.PreparedCoffee), Is.EqualTo(1));
            Assert.That(till.TotalCents, Is.EqualTo(startingTillTotal + 275));
            Assert.That(runtime.State.MoneyAccountsById[customerAccountId].BalanceCents, Is.EqualTo(startingCustomerBalance - 275));
            Assert.That(runtime.State.MoneyAccountsById[cafeAccountId].BalanceCents, Is.EqualTo(startingCafeBalance + 275));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CafeSaleCompleted).Count, Is.EqualTo(1));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CashChangeMade).Count, Is.EqualTo(1));
        }

        [Test]
        public void CafeVisit_FollowsOrderPrepReceivePayChatLeaveFlow()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");
            var customerAccountId = new WorldEntityId("account_holland_cash");
            var cafeAccountId = new WorldEntityId("account_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(new WorldEntityId("application_noah_cafe_visit"), new WorldEntityId("posting_cafe_worker_001"), workerId));
            runtime.Execute(new OfferJobCommand(new WorldEntityId("application_noah_cafe_visit"), workerId));
            runtime.Execute(new AcceptJobOfferCommand(new WorldEntityId("contract_noah_cafe_visit"), new WorldEntityId("application_noah_cafe_visit"), workerId));
            runtime.Execute(new StartShiftCommand(new WorldEntityId("shift_noah_cafe_visit"), new WorldEntityId("contract_noah_cafe_visit"), 120));
            runtime.Execute(new AdvanceTimeCommand(100));

            Assert.That(runtime.State.TryGetNextQueuedTask(workplaceId, WorldActionKind.ServeCustomer, out JobTaskState task), Is.True);
            Assert.That(runtime.State.TryGetVisitForTask(task.Id, out VisitState visit), Is.True);
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.Enter));

            WorldCommandResult order = runtime.Execute(new TakeCafeOrderCommand(visit.Id, workerId, CafeRecipeCatalog.HouseCoffee));
            Assert.That(order.Succeeded, Is.True, order.Message);
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.Order));

            runtime.State.TryGetInventoryContainerForOwner(workplaceId, InventoryContainerKind.WorkplaceStorage, out InventoryContainerState inventory);
            int startingBeans = inventory.CountOf(ItemCatalog.CoffeeBeans);
            int startingCustomerBalance = runtime.State.MoneyAccountsById[customerAccountId].BalanceCents;
            int startingCafeBalance = runtime.State.MoneyAccountsById[cafeAccountId].BalanceCents;

            Assert.That(runtime.Execute(new StartJobTaskCommand(task.Id, new WorldEntityId("shift_noah_cafe_visit"), workerId)).Succeeded, Is.True);
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.AwaitPrep));
            Assert.That(runtime.Execute(new SubmitMiniGameResultCommand(task.Id, workerId, 92, 100, 35, 0)).Succeeded, Is.True);
            WorldCommandResult prep = runtime.Execute(new CompleteJobTaskCommand(task.Id, workerId));

            Assert.That(prep.Succeeded, Is.True, prep.Message);
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.Receive));
            Assert.That(visit.PrepQuality, Is.EqualTo(92));
            Assert.That(inventory.CountOf(ItemCatalog.CoffeeBeans), Is.EqualTo(startingBeans - 1));
            Assert.That(inventory.CountOf(ItemCatalog.PreparedCoffee), Is.EqualTo(0));

            WorldCommandResult payment = runtime.Execute(new PayCafeVisitCommand(
                visit.Id,
                workerId,
                new[] { new CashDenominationStackState(CashDenomination.FiveDollar, 1) }));
            Assert.That(payment.Succeeded, Is.True, payment.Message);
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.Pay));
            Assert.That(runtime.State.MoneyAccountsById[customerAccountId].BalanceCents, Is.EqualTo(startingCustomerBalance - 275));
            Assert.That(runtime.State.MoneyAccountsById[cafeAccountId].BalanceCents, Is.EqualTo(startingCafeBalance + 275));
            Assert.That(runtime.State.BusinessLedgerEntries.Count, Is.EqualTo(1));

            Assert.That(runtime.Execute(new ChatWithCafeCustomerCommand(visit.Id, workerId, "morning")).Succeeded, Is.True);
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.Chat));

            WorldCommandResult leave = runtime.Execute(new LeaveCafeVisitCommand(visit.Id, workerId));
            Assert.That(leave.Succeeded, Is.True, leave.Message);
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.Leave));
            Assert.That(visit.Status, Is.EqualTo(VisitStatus.Served));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CafeOrderTaken).Count, Is.EqualTo(1));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.VisitCompleted).Count, Is.EqualTo(1));

            WorldState loaded = WorldSaveMapper.ToRuntime(WorldSaveMapper.ToSaveData(runtime.State));
            Assert.That(loaded.VisitsById[visit.Id].CafeStage, Is.EqualTo(CafeVisitStage.Leave));
            Assert.That(loaded.VisitsById[visit.Id].RecipeId, Is.EqualTo(CafeRecipeCatalog.HouseCoffee));
        }

        [Test]
        public void WorldSaveMapper_RoundTripsInventoryContainersAndCashDrawers()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();

            WorldState loaded = WorldSaveMapper.ToRuntime(WorldSaveMapper.ToSaveData(state));

            Assert.That(loaded.TryGetInventoryContainer(new WorldEntityId("inv_linden_cafe_storage"), out InventoryContainerState inventory), Is.True);
            Assert.That(inventory.CountOf(ItemCatalog.PaperCup), Is.EqualTo(30));
            Assert.That(loaded.TryGetCashDrawer(new WorldEntityId("cash_linden_cafe_till"), out CashDrawerState till), Is.True);
            Assert.That(till.CountOf(CashDenomination.Quarter), Is.EqualTo(40));
        }
    }
}
