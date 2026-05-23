using Legacy.Commands;
using Legacy.Save;
using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldBootstrap : MonoBehaviour
    {
        public static WorldRuntime Runtime { get; private set; }
        public static SaveManager SaveManager { get; private set; }

        private void Awake()
        {
            if (Runtime != null) {
                return;
            }

            Runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            SaveManager = new SaveManager(new SavePaths(Application.persistentDataPath));
            DontDestroyOnLoad(gameObject);
        }
    }
}
