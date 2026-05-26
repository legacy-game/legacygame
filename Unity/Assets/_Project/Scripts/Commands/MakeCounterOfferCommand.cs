using Legacy.World;

namespace Legacy.Commands
{
    public sealed class MakeCounterOfferCommand : IWorldCommand
    {
        private readonly WorldEntityId _applicationId;
        private readonly WorldEntityId _actorId;
        private readonly int _requestedPayCents;

        public MakeCounterOfferCommand(WorldEntityId applicationId, WorldEntityId actorId, int requestedPayCents)
        {
            _applicationId = applicationId;
            _actorId = actorId;
            _requestedPayCents = requestedPayCents;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobApplication(_applicationId, out JobApplicationState application)) {
                return WorldCommandResult.Failure("Application not found.");
            }

            if (application.ApplicantCitizenId != _actorId) {
                return WorldCommandResult.Failure("Only the applicant can counteroffer for now.");
            }

            return WorldCommandResult.Success($"Counteroffer noted at {EconomySystem.FormatMoney(_requestedPayCents)}. Negotiation UI can display this value.");
        }
    }
}
