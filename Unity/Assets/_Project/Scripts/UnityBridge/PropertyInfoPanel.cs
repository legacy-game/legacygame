using UnityEngine;
using Legacy.History;
using Legacy.World;

namespace Legacy.UnityBridge
{
    public sealed class PropertyInfoPanel : MonoBehaviour
    {
        private string _message = "WASD to move. E at doors. F1 debug.";

        public void Show(string message)
        {
            _message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 360, 126), GUI.skin.box);
            GUILayout.Label("legacy smoke");
            GUILayout.Space(4);
            GUILayout.Label($"Objective: {GetObjectiveText()}");
            GUILayout.Space(4);
            GUILayout.Label(_message);
            GUILayout.Space(8);
            GUILayout.Label("E interact | F1 debug | Y inspect land | U claim land");
            GUILayout.EndArea();
        }

        private string GetObjectiveText()
        {
            if (WorldBootstrap.Runtime == null) {
                return "World not loaded.";
            }

            WorldState state = WorldBootstrap.Runtime.State;
            var playerId = new WorldEntityId("citizen_noaharan");
            if (!state.TryGetCitizenRegistration(playerId, out CitizenRegistrationState _)) {
                return "Find the blue civic desk and register.";
            }

            if (state.GetHistoryByKind(HistoryEventKind.WorldActionPerformed).Count == 0) {
                return "Visit Linden Cafe and use the counter.";
            }

            return "Opening complete. Explore.";
        }
    }
}
