using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldScenePanel : MonoBehaviour
    {
        private void OnGUI()
        {
            if (!WorldDebugPanel.IsVisible) {
                return;
            }

            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            string sceneName = state.TryGetScene(state.CurrentSceneId, out WorldSceneState scene)
                ? scene.DisplayName
                : state.CurrentSceneId.ToString();

            GUILayout.BeginArea(new Rect(16, 390, 360, 96), GUI.skin.box);
            GUILayout.Label($"Active world scene: {sceneName}");
            GUILayout.Label("1 = Linden Street exterior");
            GUILayout.Label("2 = Linden Cafe interior");
            GUILayout.Label("3 = Pell Pharmacy interior");
            GUILayout.EndArea();
        }
    }
}
