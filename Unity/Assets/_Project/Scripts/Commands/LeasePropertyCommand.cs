using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class LeasePropertyCommand : IWorldCommand
    {
        private readonly WorldEntityId _leaseId;
        private readonly WorldEntityId _tenantRecordId;
        private readonly WorldEntityId _buildingId;
        private readonly WorldEntityId _tenantCitizenId;
        private readonly int _rentCents;
        private readonly int _dueDayOfMonth;
        private readonly WorldEntityId _actorId;

        public LeasePropertyCommand(
            WorldEntityId leaseId,
            WorldEntityId tenantRecordId,
            WorldEntityId buildingId,
            WorldEntityId tenantCitizenId,
            int rentCents,
            int dueDayOfMonth,
            WorldEntityId actorId)
        {
            _leaseId = leaseId;
            _tenantRecordId = tenantRecordId;
            _buildingId = buildingId;
            _tenantCitizenId = tenantCitizenId;
            _rentCents = rentCents;
            _dueDayOfMonth = dueDayOfMonth;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (_rentCents <= 0) {
                return WorldCommandResult.Failure("Rent must be positive.");
            }

            if (_dueDayOfMonth < 1 || _dueDayOfMonth > 31) {
                return WorldCommandResult.Failure("Rent due day must be 1-31.");
            }

            if (!context.State.TryGetBuilding(_buildingId, out BuildingState building)) {
                return WorldCommandResult.Failure($"Building not found: {_buildingId}");
            }

            if (building.OwnerCitizenId != _actorId) {
                return WorldCommandResult.Failure("Only the owner can lease this building.");
            }

            if (!context.State.TryGetCitizen(_tenantCitizenId, out CitizenState tenant)) {
                return WorldCommandResult.Failure($"Citizen not found: {_tenantCitizenId}");
            }

            if (!context.State.TryGetMoneyAccountForOwner(_tenantCitizenId, out MoneyAccountState _)) {
                return WorldCommandResult.Failure("Tenant account not found.");
            }

            if (context.State.TryGetActiveLeaseForBuilding(_buildingId, out LeaseContractState _)) {
                return WorldCommandResult.Failure("Building already has an active lease.");
            }

            var lease = new LeaseContractState(
                _leaseId,
                building.Id,
                building.OwnerCitizenId,
                tenant.Id,
                _rentCents,
                _dueDayOfMonth,
                context.State.CurrentTime);
            var tenantRecord = new TenantRecordState(
                _tenantRecordId,
                lease.Id,
                building.Id,
                tenant.Id,
                context.State.CurrentTime);

            context.State.AddLeaseContract(lease);
            context.State.AddTenantRecord(tenantRecord);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.LeaseSigned,
                $"{tenant.DisplayName} leased {building.DisplayName} for {EconomySystem.FormatMoney(_rentCents)} due day {_dueDayOfMonth}.",
                new[] { _actorId, tenant.Id },
                new[] { building.Id, building.InteriorPlaceId });

            return WorldCommandResult
                .Success($"{tenant.DisplayName} leased {building.DisplayName}.")
                .WithChangedEntity(lease.Id)
                .WithChangedEntity(tenantRecord.Id)
                .WithChangedEntity(building.Id)
                .WithHistoryEvent(history);
        }
    }
}
