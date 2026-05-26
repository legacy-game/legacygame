using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class ReceiveDeliveryCommand : IWorldCommand
    {
        private readonly WorldEntityId _workplaceId;
        private readonly string _itemId;
        private readonly int _count;
        private readonly WorldEntityId _actorId;

        public ReceiveDeliveryCommand(WorldEntityId workplaceId, string itemId, int count, WorldEntityId actorId)
        {
            _workplaceId = workplaceId;
            _itemId = itemId;
            _count = count;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetWorkplace(_workplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure("Workplace not found.");
            }

            if (!context.State.TryGetWorkplaceInventory(workplace.Id, out WorkplaceInventoryState inventory)) {
                inventory = new WorkplaceInventoryState(workplace.Id);
                context.State.AddWorkplaceInventory(inventory);
            }

            inventory.Add(_itemId, _count);
            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.DeliveryReceived, $"{_count} {_itemId} delivered.", new[] { _actorId }, new[] { workplace.PlaceId });
            return WorldCommandResult.Success("Delivery received.").WithChangedEntity(workplace.Id).WithHistoryEvent(history);
        }
    }
}
