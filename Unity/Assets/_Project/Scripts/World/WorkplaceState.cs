using System;
using System.Collections.Generic;

namespace Legacy.World
{
    [Serializable]
    public sealed class WorkplaceState
    {
        private readonly List<WorldEntityId> _activeShiftIds = new();
        private readonly List<WorldEntityId> _queuedTaskIds = new();

        public WorldEntityId Id { get; }
        public WorldEntityId BuildingId { get; }
        public WorldEntityId PlaceId { get; }
        public WorldEntityId OwnerCitizenId { get; }
        public WorldEntityId BusinessAccountId { get; }
        public WorkplaceStatus Status { get; private set; }
        public int OpensAtMinute { get; private set; }
        public int ClosesAtMinute { get; private set; }
        public IReadOnlyList<WorldEntityId> ActiveShiftIds => _activeShiftIds;
        public IReadOnlyList<WorldEntityId> QueuedTaskIds => _queuedTaskIds;

        public WorkplaceState(
            WorldEntityId id,
            WorldEntityId buildingId,
            WorldEntityId placeId,
            WorldEntityId ownerCitizenId,
            WorldEntityId businessAccountId,
            WorkplaceStatus status,
            int opensAtMinute,
            int closesAtMinute)
        {
            Id = id;
            BuildingId = buildingId;
            PlaceId = placeId;
            OwnerCitizenId = ownerCitizenId;
            BusinessAccountId = businessAccountId;
            Status = status;
            OpensAtMinute = opensAtMinute;
            ClosesAtMinute = closesAtMinute;
        }

        public void Open()
        {
            Status = WorkplaceStatus.Open;
        }

        public void Close()
        {
            Status = WorkplaceStatus.Closed;
        }

        public void SetHours(int opensAtMinute, int closesAtMinute)
        {
            OpensAtMinute = opensAtMinute;
            ClosesAtMinute = closesAtMinute;
        }

        public void AddActiveShift(WorldEntityId shiftId)
        {
            if (!_activeShiftIds.Contains(shiftId)) {
                _activeShiftIds.Add(shiftId);
            }
        }

        public void RemoveActiveShift(WorldEntityId shiftId)
        {
            _activeShiftIds.Remove(shiftId);
        }

        public void EnqueueTask(WorldEntityId taskId)
        {
            if (!_queuedTaskIds.Contains(taskId)) {
                _queuedTaskIds.Add(taskId);
            }
        }

        public void RemoveTask(WorldEntityId taskId)
        {
            _queuedTaskIds.Remove(taskId);
        }
    }
}
