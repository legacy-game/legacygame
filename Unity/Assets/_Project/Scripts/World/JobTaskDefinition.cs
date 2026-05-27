using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class JobTaskDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string RequiredRoleId { get; }
        public WorldActionKind ActionKind { get; }
        public MiniGameKind MiniGameKind { get; }
        public int BaseDurationMinutes { get; }
        public int BasePayCents { get; }
        public string InputItemId { get; }
        public int InputItemCount { get; }
        public string OutputItemId { get; }
        public int OutputItemCount { get; }
        public string RecipeId { get; }

        public JobTaskDefinition(
            string id,
            string displayName,
            string requiredRoleId,
            WorldActionKind actionKind,
            MiniGameKind miniGameKind,
            int baseDurationMinutes,
            int basePayCents,
            string inputItemId,
            int inputItemCount,
            string outputItemId,
            int outputItemCount,
            string recipeId = "")
        {
            Id = id;
            DisplayName = displayName;
            RequiredRoleId = requiredRoleId;
            ActionKind = actionKind;
            MiniGameKind = miniGameKind;
            BaseDurationMinutes = baseDurationMinutes;
            BasePayCents = basePayCents;
            InputItemId = inputItemId ?? string.Empty;
            InputItemCount = inputItemCount;
            OutputItemId = outputItemId ?? string.Empty;
            OutputItemCount = outputItemCount;
            RecipeId = recipeId ?? string.Empty;
        }
    }
}
