using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class PayRentCommand : IWorldCommand
    {
        private readonly WorldEntityId _leaseId;
        private readonly WorldEntityId _payerCitizenId;

        public PayRentCommand(WorldEntityId leaseId, WorldEntityId payerCitizenId)
        {
            _leaseId = leaseId;
            _payerCitizenId = payerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetLeaseContract(_leaseId, out LeaseContractState lease)) {
                return WorldCommandResult.Failure($"Lease not found: {_leaseId}");
            }

            if (lease.Status != LeaseContractStatus.Active) {
                return WorldCommandResult.Failure("Lease is not active.");
            }

            if (lease.TenantCitizenId != _payerCitizenId) {
                return WorldCommandResult.Failure("Only the tenant can pay this rent.");
            }

            if (!context.State.TryGetBuilding(lease.BuildingId, out BuildingState building)) {
                return WorldCommandResult.Failure("Leased building not found.");
            }

            if (!context.State.TryGetMoneyAccountForOwner(lease.TenantCitizenId, out MoneyAccountState tenantAccount)) {
                return WorldCommandResult.Failure("Tenant account not found.");
            }

            if (!context.State.TryGetMoneyAccountForOwner(lease.LandlordCitizenId, out MoneyAccountState landlordAccount)) {
                return WorldCommandResult.Failure("Landlord account not found.");
            }

            var economy = new EconomySystem(context.State);
            EconomyTransferResult transfer = economy.Transfer(
                tenantAccount.Id,
                landlordAccount.Id,
                lease.RentCents,
                $"Rent for {building.DisplayName}",
                building.InteriorPlaceId,
                WorldActionKind.PayRent,
                context.State.CurrentTime);
            if (!transfer.Succeeded) {
                return WorldCommandResult.Failure(transfer.Message);
            }

            lease.RecordPayment(context.State.CurrentTime);
            AddRentLedgerEntry(context, lease, building);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.RentPaid,
                $"{EconomySystem.FormatMoney(lease.RentCents)} rent paid for {building.DisplayName}.",
                new[] { lease.TenantCitizenId, lease.LandlordCitizenId },
                new[] { building.Id, building.InteriorPlaceId });

            return WorldCommandResult
                .Success($"{EconomySystem.FormatMoney(lease.RentCents)} rent paid for {building.DisplayName}.")
                .WithChangedEntity(lease.Id)
                .WithChangedEntity(tenantAccount.Id)
                .WithChangedEntity(landlordAccount.Id)
                .WithHistoryEvent(history);
        }

        private static void AddRentLedgerEntry(WorldCommandContext context, LeaseContractState lease, BuildingState building)
        {
            if (!context.State.TryGetWorkplaceByPlace(building.InteriorPlaceId, out WorkplaceState workplace)) {
                return;
            }

            context.State.AddBusinessLedgerEntry(new BusinessLedgerEntryState(
                context.State.CreateNextBusinessLedgerEntryId(),
                workplace.Id,
                workplace.BusinessAccountId,
                BusinessLedgerEntryKind.RentPayment,
                lease.RentCents,
                $"Rent paid for {building.DisplayName}",
                context.State.CurrentTime,
                lease.Id));
        }
    }
}
