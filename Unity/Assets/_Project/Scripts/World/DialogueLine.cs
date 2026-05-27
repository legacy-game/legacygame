using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class DialogueLine
    {
        public string Id { get; }
        public WorldEntityId SpeakerCitizenId { get; }
        public string Topic { get; }
        public string Text { get; }
        public int AffinityDelta { get; }
        public int FamiliarityDelta { get; }
        public string MemorySummary { get; }

        public DialogueLine(
            string id,
            WorldEntityId speakerCitizenId,
            string topic,
            string text,
            int affinityDelta = 0,
            int familiarityDelta = 1,
            string memorySummary = "")
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentException("Dialogue id must not be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(text)) {
                throw new ArgumentException("Dialogue text must not be empty.", nameof(text));
            }

            Id = id;
            SpeakerCitizenId = speakerCitizenId;
            Topic = topic ?? string.Empty;
            Text = text;
            AffinityDelta = affinityDelta;
            FamiliarityDelta = familiarityDelta;
            MemorySummary = memorySummary ?? string.Empty;
        }
    }
}
