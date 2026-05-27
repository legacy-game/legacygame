using Legacy.Commands;
using Legacy.History;
using Legacy.Save;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class OwnershipFoundationsTests
    {
        [Test]
        public void LeaseProperty_AddsLeaseTenantRecordAndTenantAccess()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var leaseId = new WorldEntityId("lease_cafe_rowan");
            var tenantRecordId = new WorldEntityId("tenant_cafe_rowan");
            var buildingId = new WorldEntityId("building_linden_cafe");
            var tenantId = new WorldEntityId("citizen_rowan");

            WorldCommandResult result = runtime.Execute(new LeasePropertyCommand(
                leaseId,
                tenantRecordId,
                buildingId,
                tenantId,
                850,
                14,
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.LeaseContractsById[leaseId].Status, Is.EqualTo(LeaseContractStatus.Active));
            Assert.That(runtime.State.TenantRecordsById[tenantRecordId].Status, Is.EqualTo(TenantRecordStatus.Active));
            Assert.That(new PropertySystem(runtime.State).CheckBuildingAccess(buildingId, tenantId).IsAllowed, Is.True);
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.LeaseSigned).Count, Is.EqualTo(1));
        }

        [Test]
        public void PayRent_TransfersMoneyAndRecordsLedgerExpense()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var leaseId = new WorldEntityId("lease_cafe_rowan");
            var tenantId = new WorldEntityId("citizen_rowan");
            var landlordId = new WorldEntityId("citizen_noaharan");
            int tenantStartingBalance = runtime.State.MoneyAccountsById[new WorldEntityId("account_rowan_cash")].BalanceCents;
            int landlordStartingBalance = runtime.State.MoneyAccountsById[new WorldEntityId("account_noaharan_cash")].BalanceCents;

            runtime.Execute(new LeasePropertyCommand(
                leaseId,
                new WorldEntityId("tenant_cafe_rowan"),
                new WorldEntityId("building_linden_cafe"),
                tenantId,
                850,
                14,
                landlordId));
            WorldCommandResult result = runtime.Execute(new PayRentCommand(leaseId, tenantId));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.LeaseContractsById[leaseId].PaymentsMade, Is.EqualTo(1));
            Assert.That(runtime.State.MoneyAccountsById[new WorldEntityId("account_rowan_cash")].BalanceCents, Is.EqualTo(tenantStartingBalance - 850));
            Assert.That(runtime.State.MoneyAccountsById[new WorldEntityId("account_noaharan_cash")].BalanceCents, Is.EqualTo(landlordStartingBalance + 850));
            Assert.That(runtime.State.BusinessLedgerEntries.Count, Is.EqualTo(1));
            Assert.That(runtime.State.BusinessLedgerEntries[0].Kind, Is.EqualTo(BusinessLedgerEntryKind.RentPayment));
            Assert.That(new EconomySystem(runtime.State).TryGetBusinessSummary(new WorldEntityId("workplace_linden_cafe"), out BusinessAccountingSummary summary), Is.True);
            Assert.That(summary.ExpenseCents, Is.EqualTo(850));
            Assert.That(summary.NetCents, Is.EqualTo(-850));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.RentPaid).Count, Is.EqualTo(1));
        }

        [Test]
        public void RecordBusinessIncome_CreditsBusinessAccountAndSummarizesLedger()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workplaceId = new WorldEntityId("workplace_linden_cafe");
            var accountId = new WorldEntityId("account_linden_cafe");
            int startingBalance = runtime.State.MoneyAccountsById[accountId].BalanceCents;

            WorldCommandResult result = runtime.Execute(new RecordBusinessIncomeCommand(
                workplaceId,
                675,
                "Prototype counter sales",
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.MoneyAccountsById[accountId].BalanceCents, Is.EqualTo(startingBalance + 675));
            Assert.That(runtime.State.BusinessLedgerEntries.Count, Is.EqualTo(1));
            Assert.That(runtime.State.BusinessLedgerEntries[0].Kind, Is.EqualTo(BusinessLedgerEntryKind.Income));
            Assert.That(new EconomySystem(runtime.State).TryGetBusinessSummary(workplaceId, out BusinessAccountingSummary summary), Is.True);
            Assert.That(summary.IncomeCents, Is.EqualTo(675));
            Assert.That(summary.NetCents, Is.EqualTo(675));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.BusinessIncomeRecorded).Count, Is.EqualTo(1));
        }

        [Test]
        public void SaveRoundTrip_PreservesLeaseTenantRecordsAndBusinessLedger()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var leaseId = new WorldEntityId("lease_cafe_rowan");
            var tenantRecordId = new WorldEntityId("tenant_cafe_rowan");

            runtime.Execute(new LeasePropertyCommand(
                leaseId,
                tenantRecordId,
                new WorldEntityId("building_linden_cafe"),
                new WorldEntityId("citizen_rowan"),
                850,
                14,
                new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new PayRentCommand(leaseId, new WorldEntityId("citizen_rowan")));
            runtime.Execute(new RecordBusinessIncomeCommand(
                new WorldEntityId("workplace_linden_cafe"),
                675,
                "Prototype counter sales",
                new WorldEntityId("citizen_noaharan")));

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);

            Assert.That(loaded.LeaseContractsById[leaseId].PaymentsMade, Is.EqualTo(1));
            Assert.That(loaded.TenantRecordsById[tenantRecordId].Status, Is.EqualTo(TenantRecordStatus.Active));
            Assert.That(loaded.BusinessLedgerEntries.Count, Is.EqualTo(2));
            Assert.That(new PropertySystem(loaded).CheckBuildingAccess(new WorldEntityId("building_linden_cafe"), new WorldEntityId("citizen_rowan")).IsAllowed, Is.True);
            Assert.That(new EconomySystem(loaded).TryGetBusinessSummary(new WorldEntityId("workplace_linden_cafe"), out BusinessAccountingSummary summary), Is.True);
            Assert.That(summary.IncomeCents, Is.EqualTo(675));
            Assert.That(summary.ExpenseCents, Is.EqualTo(850));
        }
    }
}
