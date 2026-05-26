using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class CloseWorkplaceCommand : IWorldCommand
    {
        private readonly WorldEntityId _workplaceId;
        private readonly WorldEntityId _actorId;

        public CloseWorkplaceCommand(WorldEntityId workplaceId, WorldEntityId actorId)
        {
            _workplaceId = workplaceId;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetWorkplace(_workplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure($"Workplace not found: {_workplaceId}");
            }

            if (workplace.OwnerCitizenId != _actorId) {
                return WorldCommandResult.Failure("Only the owner can close this workplace.");
            }

            workplace.Close();
            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.WorkplaceClosed, "A workplace closed.", new[] { _actorId }, new[] { workplace.PlaceId });
            return WorldCommandResult.Success("Workplace closed.").WithChangedEntity(workplace.Id).WithHistoryEvent(history);
        }
    }
}
