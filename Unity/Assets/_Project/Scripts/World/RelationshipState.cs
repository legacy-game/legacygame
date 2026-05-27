using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class RelationshipState
    {
        public WorldEntityId OwnerCitizenId { get; }
        public WorldEntityId OtherCitizenId { get; }
        public int Affinity { get; private set; }
        public int Familiarity { get; private set; }
        public GameDateTime LastInteractionAt { get; private set; }

        public RelationshipState(
            WorldEntityId ownerCitizenId,
            WorldEntityId otherCitizenId,
            int affinity = 0,
            int familiarity = 0,
            GameDateTime lastInteractionAt = default)
        {
            OwnerCitizenId = ownerCitizenId;
            OtherCitizenId = otherCitizenId;
            Affinity = affinity;
            Familiarity = familiarity;
            LastInteractionAt = lastInteractionAt;
        }

        public void ApplyInteraction(int affinityDelta, int familiarityDelta, GameDateTime interactedAt)
        {
            Affinity += affinityDelta;
            Familiarity += familiarityDelta;
            LastInteractionAt = interactedAt;
        }
    }
}
