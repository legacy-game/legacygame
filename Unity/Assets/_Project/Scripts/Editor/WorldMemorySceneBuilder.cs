using Legacy.UnityBridge;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Legacy.Editor
{
    public static class WorldMemorySceneBuilder
    {
        private const string SceneFolder = "Assets/_Project/Scenes";

        [MenuItem("Legacy/Build World Memory Smoke Scene")]
        public static void Build()
        {
            if (!AssetDatabase.IsValidFolder("Assets/_Project")) {
                AssetDatabase.CreateFolder("Assets", "_Project");
            }

            if (!AssetDatabase.IsValidFolder(SceneFolder)) {
                AssetDatabase.CreateFolder("Assets/_Project", "Scenes");
            }

            BuildExteriorScene();
            BuildCafeInteriorScene();

            EditorBuildSettings.scenes = new[] {
                new EditorBuildSettingsScene(WorldSceneIds.ExteriorUnityScenePath, true),
                new EditorBuildSettingsScene(WorldSceneIds.CafeInteriorUnityScenePath, true)
            };

            AssetDatabase.SaveAssets();
        }

        private static void BuildExteriorScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = WorldSceneIds.ExteriorUnitySceneName;
            CreateCommonWorldObjects(WorldSceneIds.ExteriorSceneId);

            CreateTitle("Linden Street Exterior", new Vector3(0f, 3.8f, -0.5f));

            EditorSceneManager.SaveScene(scene, WorldSceneIds.ExteriorUnityScenePath);
        }

        private static void BuildCafeInteriorScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = WorldSceneIds.CafeInteriorUnitySceneName;
            CreateCommonWorldObjects(WorldSceneIds.CafeInteriorSceneId);

            CreateTitle("Linden Cafe Interior", new Vector3(0f, 3.8f, -0.5f));

            EditorSceneManager.SaveScene(scene, WorldSceneIds.CafeInteriorUnityScenePath);
        }

        private static void CreateCommonWorldObjects(string sceneId)
        {
            Camera camera = CreateCamera();

            var bootstrap = new GameObject("WorldBootstrap");
            bootstrap.AddComponent<WorldBootstrap>();

            var sceneBridgeObject = new GameObject("WorldSceneBridge");
            WorldSceneBridge sceneBridge = sceneBridgeObject.AddComponent<WorldSceneBridge>();
            sceneBridge.SetSceneId(sceneId);

            var presenterObject = new GameObject("WorldScenePresenter");
            WorldScenePresenter presenter = presenterObject.AddComponent<WorldScenePresenter>();
            presenter.SetSceneId(sceneId);

            var debugPanel = new GameObject("WorldDebugPanel");
            debugPanel.AddComponent<WorldDebugPanel>();
            debugPanel.AddComponent<WorldScenePanel>();

            var propertyPanelObject = new GameObject("PropertyInfoPanel");
            PropertyInfoPanel propertyInfoPanel = propertyPanelObject.AddComponent<PropertyInfoPanel>();

            WorldInputBridge worldInputBridge = debugPanel.AddComponent<WorldInputBridge>();
            worldInputBridge.SetInfoPanel(propertyInfoPanel);

            var propertyInteractionObject = new GameObject("PropertyInteractionBridge");
            PropertyInteractionBridge propertyInteraction = propertyInteractionObject.AddComponent<PropertyInteractionBridge>();
            propertyInteraction.SetCamera(camera);
            propertyInteraction.SetInfoPanel(propertyInfoPanel);
        }

        private static Camera CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.backgroundColor = new Color(0.12f, 0.12f, 0.14f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            return camera;
        }

        private static void CreateTitle(string text, Vector3 position)
        {
            var titleObject = new GameObject("SceneTitle");
            titleObject.transform.position = position;

            TextMesh title = titleObject.AddComponent<TextMesh>();
            title.text = text;
            title.anchor = TextAnchor.MiddleCenter;
            title.alignment = TextAlignment.Center;
            title.fontSize = 32;
            title.color = Color.white;
        }
    }
}
