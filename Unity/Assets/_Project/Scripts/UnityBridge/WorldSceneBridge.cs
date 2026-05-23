using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldSceneBridge : MonoBehaviour
    {
        [SerializeField] private string _sceneId = WorldSceneIds.ExteriorSceneId;

        public void SetSceneId(string sceneId)
        {
            _sceneId = sceneId;
        }

        private void Start()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            var sceneId = new WorldEntityId(_sceneId);
            if (WorldBootstrap.Runtime.State.CurrentSceneId != sceneId) {
                // Direct editor Play Mode entry should bind the scene without creating a travel history event.
                WorldBootstrap.Runtime.State.SetCurrentScene(sceneId);
            }
        }
    }
}
