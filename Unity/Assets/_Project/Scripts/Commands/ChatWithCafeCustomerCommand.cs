using Legacy.World;

namespace Legacy.Commands
{
    public sealed class ChatWithCafeCustomerCommand : IWorldCommand
    {
        private readonly WorldEntityId _visitId;
        private readonly WorldEntityId _workerCitizenId;
        private readonly string _topic;
        private readonly string _lineId;

        public ChatWithCafeCustomerCommand(WorldEntityId visitId, WorldEntityId workerCitizenId, string topic = "", string lineId = "")
        {
            _visitId = visitId;
            _workerCitizenId = workerCitizenId;
            _topic = topic ?? string.Empty;
            _lineId = lineId ?? string.Empty;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetVisit(_visitId, out VisitState visit)) {
                return WorldCommandResult.Failure($"Visit not found: {_visitId}");
            }

            if (!visit.IsCafeVisit || visit.CafeStage != CafeVisitStage.Pay) {
                return WorldCommandResult.Failure($"Cafe visit must be at Pay before customer chat; current stage is {visit.CafeStage}.");
            }

            WorldCommandResult result = new TalkToCitizenCommand(_workerCitizenId, visit.VisitorCitizenId, _topic, _lineId).Execute(context);
            if (!result.Succeeded) {
                return result;
            }

            visit.MarkCafeChatted();
            return result
                .WithMessage($"Cafe chat complete. {result.Message}")
                .WithChangedEntity(visit.Id);
        }
    }
}
