using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class VisitState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId VisitorCitizenId { get; }
        public WorldEntityId WorkplaceId { get; }
        public WorldEntityId PlaceId { get; }
        public string Intent { get; }
        public string RequestedTaskDefinitionId { get; }
        public WorldEntityId LinkedTaskId { get; private set; }
        public VisitStatus Status { get; private set; }
        public GameDateTime ArrivalTime { get; }
        public GameDateTime DepartureTime { get; private set; }
        public string ArrivalLine { get; }
        public string CompletionLine { get; }

        public VisitState(
            WorldEntityId id,
            WorldEntityId visitorCitizenId,
            WorldEntityId workplaceId,
            WorldEntityId placeId,
            string intent,
            string requestedTaskDefinitionId,
            WorldEntityId linkedTaskId,
            VisitStatus status,
            GameDateTime arrivalTime,
            GameDateTime departureTime,
            string arrivalLine,
            string completionLine)
        {
            Id = id;
            VisitorCitizenId = visitorCitizenId;
            WorkplaceId = workplaceId;
            PlaceId = placeId;
            Intent = intent ?? string.Empty;
            RequestedTaskDefinitionId = requestedTaskDefinitionId ?? string.Empty;
            LinkedTaskId = linkedTaskId;
            Status = status;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
            ArrivalLine = arrivalLine ?? string.Empty;
            CompletionLine = completionLine ?? string.Empty;
        }

        public void LinkTask(WorldEntityId taskId)
        {
            LinkedTaskId = taskId;
            Status = VisitStatus.WaitingForTask;
        }

        public void MarkServed(GameDateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = VisitStatus.Served;
        }

        public void MarkLeft(GameDateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = VisitStatus.Left;
        }

        public void MarkFailed(GameDateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = VisitStatus.Failed;
        }
    }
}
