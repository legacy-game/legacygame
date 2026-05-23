using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class PropertyInfoPanel : MonoBehaviour
    {
        private string _message = "Click the Linden Cafe block to inspect ownership.";

        public void Show(string message)
        {
            _message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 220, 360, 160), GUI.skin.box);
            GUILayout.Label("Property & Place");
            GUILayout.Space(4);
            GUILayout.Label(_message);
            GUILayout.Space(8);
            GUILayout.Label("Click building = inspect\nT = advance time\nI = inspect default building");
            GUILayout.EndArea();
        }
    }
}
