namespace Legacy.World
{
    public static class DialogueCatalog
    {
        public const string RowanMorningGreeting = "dialogue_rowan_morning_greeting";
        public const string PellMorningGreeting = "dialogue_pell_morning_greeting";
        public const string GenericGreeting = "dialogue_generic_greeting";

        private static readonly DialogueLine[] Lines = {
            new DialogueLine(
                RowanMorningGreeting,
                new WorldEntityId("citizen_rowan"),
                "morning",
                "Morning. The cafe is awake before the street is.",
                1,
                1,
                "Talked about the early cafe morning."),
            new DialogueLine(
                PellMorningGreeting,
                new WorldEntityId("citizen_old_mr_pell"),
                "morning",
                "Keep an eye on the pharmacy shelves today. People remember shortages.",
                0,
                1,
                "Mentioned watching the pharmacy shelves."),
            new DialogueLine(
                GenericGreeting,
                default,
                "greeting",
                "Good to see you out in Veyne.",
                0,
                1,
                "Exchanged a simple greeting.")
        };

        public static bool TryGet(string id, out DialogueLine line)
        {
            for (int i = 0; i < Lines.Length; i++) {
                if (Lines[i].Id == id) {
                    line = Lines[i];
                    return true;
                }
            }

            line = null;
            return false;
        }

        public static DialogueLine SelectLine(WorldEntityId speakerCitizenId, string topic = "")
        {
            for (int i = 0; i < Lines.Length; i++) {
                if (Lines[i].SpeakerCitizenId == speakerCitizenId && MatchesTopic(Lines[i], topic)) {
                    return Lines[i];
                }
            }

            for (int i = 0; i < Lines.Length; i++) {
                if (Lines[i].SpeakerCitizenId == speakerCitizenId) {
                    return Lines[i];
                }
            }

            return Lines[Lines.Length - 1];
        }

        private static bool MatchesTopic(DialogueLine line, string topic)
        {
            return string.IsNullOrWhiteSpace(topic) || line.Topic == topic;
        }
    }
}
