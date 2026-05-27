using Legacy.World;

namespace Legacy.Commands
{
    public sealed class RentPropertyCommand : IWorldCommand
    {
        private readonly LeasePropertyCommand _lease;

        public RentPropertyCommand(
            WorldEntityId leaseId,
            WorldEntityId tenantRecordId,
            WorldEntityId buildingId,
            WorldEntityId tenantCitizenId,
            int rentCents,
            int dueDayOfMonth,
            WorldEntityId actorId)
        {
            _lease = new LeasePropertyCommand(leaseId, tenantRecordId, buildingId, tenantCitizenId, rentCents, dueDayOfMonth, actorId);
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            return _lease.Execute(context);
        }
    }
}
