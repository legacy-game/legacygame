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
        public CafeVisitStage CafeStage { get; private set; }
        public string RecipeId { get; private set; }
        public string PreparedItemId { get; private set; }
        public int PriceCents { get; private set; }
        public int PrepQuality { get; private set; }
        public int TenderedCents { get; private set; }
        public GameDateTime ArrivalTime { get; }
        public GameDateTime DepartureTime { get; private set; }
        public string ArrivalLine { get; }
        public string CompletionLine { get; }
        public bool IsCafeVisit => RequestedTaskDefinitionId == JobTaskCatalog.ServeCafeCustomer || !string.IsNullOrEmpty(RecipeId);

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
            string completionLine,
            CafeVisitStage cafeStage = CafeVisitStage.Enter,
            string recipeId = "",
            string preparedItemId = "",
            int priceCents = 0,
            int prepQuality = 0,
            int tenderedCents = 0)
        {
            Id = id;
            VisitorCitizenId = visitorCitizenId;
            WorkplaceId = workplaceId;
            PlaceId = placeId;
            Intent = intent ?? string.Empty;
            RequestedTaskDefinitionId = requestedTaskDefinitionId ?? string.Empty;
            LinkedTaskId = linkedTaskId;
            Status = status;
            CafeStage = cafeStage;
            RecipeId = recipeId ?? string.Empty;
            PreparedItemId = preparedItemId ?? string.Empty;
            PriceCents = priceCents;
            PrepQuality = prepQuality;
            TenderedCents = tenderedCents;
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

        public void PlaceCafeOrder(CafeRecipeDefinition recipe)
        {
            RecipeId = recipe.Id;
            PreparedItemId = recipe.OutputItemId;
            PriceCents = recipe.PriceCents;
            CafeStage = CafeVisitStage.Order;
        }

        public void AwaitCafePrep()
        {
            CafeStage = CafeVisitStage.AwaitPrep;
        }

        public void ReceiveCafeOrder(int quality)
        {
            PrepQuality = quality;
            CafeStage = CafeVisitStage.Receive;
        }

        public void MarkCafePaid(int tenderedCents)
        {
            TenderedCents = tenderedCents;
            CafeStage = CafeVisitStage.Pay;
        }

        public void MarkCafeChatted()
        {
            CafeStage = CafeVisitStage.Chat;
        }

        public void MarkCafeLeft(GameDateTime departureTime)
        {
            DepartureTime = departureTime;
            CafeStage = CafeVisitStage.Leave;
            Status = VisitStatus.Served;
        }

        public void MarkServed(GameDateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = VisitStatus.Served;
            if (IsCafeVisit) {
                CafeStage = CafeVisitStage.Leave;
            }
        }

        public void MarkLeft(GameDateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = VisitStatus.Left;
            if (IsCafeVisit) {
                CafeStage = CafeVisitStage.Leave;
            }
        }

        public void MarkFailed(GameDateTime departureTime)
        {
            DepartureTime = departureTime;
            Status = VisitStatus.Failed;
            if (IsCafeVisit) {
                CafeStage = CafeVisitStage.Leave;
            }
        }
    }
}
