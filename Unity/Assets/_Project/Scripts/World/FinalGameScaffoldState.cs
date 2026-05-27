using System;
using Legacy.Time;

namespace Legacy.World
{
    public enum FinalGameScaffoldKind
    {
        FamilyLegacy,
        HealthDeath,
        GovernmentLaws,
        CrimeJustice,
        FrontierSettlement,
        CultureArtifacts,
        Moderation
    }

    [Serializable]
    public sealed class FinalGameScaffoldState
    {
        public WorldEntityId Id { get; }
        public FinalGameScaffoldKind Kind { get; }
        public string DisplayName { get; }
        public string Summary { get; }
        public string ImplementationNotes { get; }
        public GameDateTime CreatedAt { get; }
        public bool IsGameplayEnabled { get; }

        public FinalGameScaffoldState(
            WorldEntityId id,
            FinalGameScaffoldKind kind,
            string displayName,
            string summary,
            string implementationNotes,
            GameDateTime createdAt,
            bool isGameplayEnabled = false)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            Kind = kind;
            DisplayName = displayName;
            Summary = summary ?? string.Empty;
            ImplementationNotes = implementationNotes ?? string.Empty;
            CreatedAt = createdAt;
            IsGameplayEnabled = isGameplayEnabled;
        }
    }
}
