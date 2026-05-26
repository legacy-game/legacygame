using Legacy.UnityBridge;
using Legacy.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Legacy.Editor
{
    public static class WorldMemorySceneBuilder
    {
        private const string SceneFolder = "Assets/_Project/Scenes";
        private static readonly Color ExteriorGround = new(0.16f, 0.22f, 0.18f);
        private static readonly Color StreetAsphalt = new(0.12f, 0.13f, 0.15f);
        private static readonly Color Sidewalk = new(0.28f, 0.27f, 0.24f);
        private static readonly Color InteriorFloor = new(0.20f, 0.16f, 0.13f);
        private static readonly Color InteriorWall = new(0.26f, 0.20f, 0.17f);
        private static readonly Color WarmLight = new(0.90f, 0.78f, 0.56f);

        [MenuItem("Legacy/Build World Memory Smoke Scene")]
        public static void Build()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) {
                EditorUtility.DisplayDialog(
                    "Exit Play Mode",
                    "Stop Play Mode before rebuilding the World Memory smoke scenes.",
                    "OK");
                return;
            }

            if (!AssetDatabase.IsValidFolder("Assets/_Project")) {
                AssetDatabase.CreateFolder("Assets", "_Project");
            }

            if (!AssetDatabase.IsValidFolder(SceneFolder)) {
                AssetDatabase.CreateFolder("Assets/_Project", "Scenes");
            }

            BuildExteriorScene();
            BuildCafeInteriorScene();
            BuildPharmacyInteriorScene();

            EditorBuildSettings.scenes = new[] {
                new EditorBuildSettingsScene(WorldSceneIds.ExteriorUnityScenePath, true),
                new EditorBuildSettingsScene(WorldSceneIds.CafeInteriorUnityScenePath, true),
                new EditorBuildSettingsScene(WorldSceneIds.PharmacyInteriorUnityScenePath, true)
            };

            AssetDatabase.SaveAssets();
        }

        private static void BuildExteriorScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = WorldSceneIds.ExteriorUnitySceneName;
            CreateCommonWorldObjects(WorldSceneIds.ExteriorSceneId, new Vector3(-3.8f, -1.6f, -0.4f), new Vector2(-8f, -4.6f), new Vector2(8f, 4.6f));
            CreateExteriorEnvironment();

            EditorSceneManager.SaveScene(scene, WorldSceneIds.ExteriorUnityScenePath);
        }

        private static void BuildCafeInteriorScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = WorldSceneIds.CafeInteriorUnitySceneName;
            PropertyInfoPanel propertyInfoPanel = CreateCommonWorldObjects(WorldSceneIds.CafeInteriorSceneId, new Vector3(-2.2f, -1.2f, -0.4f), new Vector2(-5.5f, -3.2f), new Vector2(5.5f, 3.2f));
            CreateInteriorEnvironment(
                new Color(0.35f, 0.20f, 0.13f),
                WorldActionKind.ServeCustomer,
                "place_linden_cafe_interior",
                "E: serve customer",
                propertyInfoPanel);

            EditorSceneManager.SaveScene(scene, WorldSceneIds.CafeInteriorUnityScenePath);
        }

        private static void BuildPharmacyInteriorScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = WorldSceneIds.PharmacyInteriorUnitySceneName;
            PropertyInfoPanel propertyInfoPanel = CreateCommonWorldObjects(WorldSceneIds.PharmacyInteriorSceneId, new Vector3(-2.2f, -1.2f, -0.4f), new Vector2(-5.5f, -3.2f), new Vector2(5.5f, 3.2f));
            CreateInteriorEnvironment(
                new Color(0.18f, 0.30f, 0.26f),
                WorldActionKind.StockShelves,
                "place_pell_pharmacy_interior",
                "E: try stock pharmacy shelves",
                propertyInfoPanel);

            EditorSceneManager.SaveScene(scene, WorldSceneIds.PharmacyInteriorUnityScenePath);
        }

        private static PropertyInfoPanel CreateCommonWorldObjects(string sceneId, Vector3 playerStart, Vector2 minBounds, Vector2 maxBounds)
        {
            Camera camera = CreateCamera();
            GameObject player = CreatePlayer(playerStart, minBounds, maxBounds);
            CameraFollow2D follow = camera.gameObject.AddComponent<CameraFollow2D>();
            follow.SetTarget(player.transform);

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
            player.GetComponent<PlayerMovementController>().SetInfoPanel(propertyInfoPanel);
            presenter.SetInfoPanel(propertyInfoPanel);

            WorldInputBridge worldInputBridge = debugPanel.AddComponent<WorldInputBridge>();
            worldInputBridge.SetInfoPanel(propertyInfoPanel);

            var propertyInteractionObject = new GameObject("PropertyInteractionBridge");
            PropertyInteractionBridge propertyInteraction = propertyInteractionObject.AddComponent<PropertyInteractionBridge>();
            propertyInteraction.SetCamera(camera);
            propertyInteraction.SetInfoPanel(propertyInfoPanel);

            CreateDoors(sceneId, propertyInfoPanel);
            return propertyInfoPanel;
        }

        private static GameObject CreatePlayer(Vector3 position, Vector2 minBounds, Vector2 maxBounds)
        {
            GameObject player = CreateQuad("Player", position, new Vector3(0.55f, 0.8f, 1f), new Color(0.82f, 0.72f, 0.45f));
            PlayerMovementController movement = player.AddComponent<PlayerMovementController>();
            movement.SetBounds(minBounds, maxBounds);
            Rigidbody2D body = player.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            body.gravityScale = 0f;
            BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;

            return player;
        }

        private static void CreateDoors(string sceneId, PropertyInfoPanel infoPanel)
        {
            if (sceneId == WorldSceneIds.ExteriorSceneId) {
                CreateRegistryStation(infoPanel);
                CreateDoor(
                    "CafeDoor",
                    new Vector3(-1.3f, -0.1f, -0.55f),
                    new Vector3(1.25f, 0.6f, 1f),
                    new Color(0.68f, 0.46f, 0.24f),
                    WorldSceneIds.CafeInteriorSceneId,
                    WorldSceneIds.CafeInteriorUnitySceneName,
                    WorldSceneIds.CafeInteriorUnityScenePath,
                    "Press E to enter Linden Cafe",
                    infoPanel);
                CreateDoor(
                    "PharmacyDoor",
                    new Vector3(2.3f, -0.1f, -0.55f),
                    new Vector3(1.25f, 0.6f, 1f),
                    new Color(0.36f, 0.58f, 0.46f),
                    WorldSceneIds.PharmacyInteriorSceneId,
                    WorldSceneIds.PharmacyInteriorUnitySceneName,
                    WorldSceneIds.PharmacyInteriorUnityScenePath,
                    "Press E to enter Pell Pharmacy",
                    infoPanel);
                return;
            }

            CreateDoor(
                "ExitDoor",
                new Vector3(0f, -2.45f, -0.55f),
                new Vector3(1.8f, 0.65f, 1f),
                new Color(0.60f, 0.50f, 0.32f),
                WorldSceneIds.ExteriorSceneId,
                WorldSceneIds.ExteriorUnitySceneName,
                WorldSceneIds.ExteriorUnityScenePath,
                "Press E to return to Linden Street",
                infoPanel);
        }

        private static void CreateRegistryStation(PropertyInfoPanel infoPanel)
        {
            GameObject registry = CreateQuad(
                "CivicRegistryDesk",
                new Vector3(-4.0f, -0.55f, -0.55f),
                new Vector3(1.7f, 0.9f, 1f),
                new Color(0.14f, 0.24f, 0.62f));
            BoxCollider2D collider = registry.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(1.2f, 1.1f);
            RegistryStationView station = registry.AddComponent<RegistryStationView>();
            station.Configure(infoPanel);
            InteractableView interactable = registry.AddComponent<InteractableView>();
            interactable.Configure(station.GetPrompt, station.Interact, 75, 1.8f);
            CreateWorldLabel(registry.transform, "Register", new Vector3(0f, -0.7f, -0.05f));
        }

        private static void CreateDoor(
            string name,
            Vector3 position,
            Vector3 scale,
            Color color,
            string targetWorldSceneId,
            string targetUnitySceneName,
            string targetUnityScenePath,
            string prompt,
            PropertyInfoPanel infoPanel)
        {
            GameObject door = CreateQuad(name, position, scale, color);
            BoxCollider2D trigger = door.AddComponent<BoxCollider2D>();
            trigger.size = new Vector2(1.35f, 0.85f);
            trigger.isTrigger = true;
            SceneDoorTrigger doorTrigger = door.AddComponent<SceneDoorTrigger>();
            doorTrigger.Configure(targetWorldSceneId, targetUnitySceneName, targetUnityScenePath, prompt, infoPanel);
            InteractableView interactable = door.AddComponent<InteractableView>();
            interactable.Configure(doorTrigger.GetPrompt, doorTrigger.Interact, 100, 2.2f);
            CreateWorldLabel(door.transform, name.Replace("Door", ""), new Vector3(0f, -0.55f, -0.05f));
        }

        private static void CreateExteriorEnvironment()
        {
            CreateQuad("GrassBlock", Vector3.forward * 0.8f, new Vector3(18f, 10f, 1f), ExteriorGround);
            CreateQuad("LindenRoad", new Vector3(0f, -1.1f, 0.65f), new Vector3(18f, 2.1f, 1f), StreetAsphalt);
            CreateQuad("NorthSidewalk", new Vector3(0f, 0.25f, 0.6f), new Vector3(18f, 0.45f, 1f), Sidewalk);
            CreateQuad("SouthSidewalk", new Vector3(0f, -2.45f, 0.6f), new Vector3(18f, 0.45f, 1f), Sidewalk);
            CreateQuad("BookshopLot", new Vector3(-4.6f, 0.85f, 0.55f), new Vector3(2.4f, 2.35f, 1f), new Color(0.18f, 0.16f, 0.21f));
            CreateQuad("CafeLot", new Vector3(-1.3f, 0.85f, 0.55f), new Vector3(2.7f, 2.35f, 1f), new Color(0.21f, 0.18f, 0.15f));
            CreateQuad("PharmacyLot", new Vector3(2.3f, 0.85f, 0.55f), new Vector3(2.7f, 2.35f, 1f), new Color(0.17f, 0.20f, 0.18f));
            CreateStreetMarker(-7.2f);
            CreateStreetMarker(-4.8f);
            CreateStreetMarker(-2.4f);
            CreateStreetMarker(0f);
            CreateStreetMarker(2.4f);
            CreateStreetMarker(4.8f);
            CreateStreetMarker(7.2f);
        }

        private static void CreateInteriorEnvironment(Color accent, WorldActionKind action, string targetPlaceId, string prompt, PropertyInfoPanel infoPanel)
        {
            CreateQuad("RoomFloor", Vector3.forward * 0.8f, new Vector3(12f, 7f, 1f), InteriorFloor);
            CreateQuad("BackWall", new Vector3(0f, 2.15f, 0.6f), new Vector3(11f, 1.5f, 1f), InteriorWall);
            GameObject counter = CreateQuad("Counter", new Vector3(0f, -0.8f, 0.45f), new Vector3(4.6f, 0.7f, 1f), accent);
            BoxCollider2D counterCollider = counter.AddComponent<BoxCollider2D>();
            counterCollider.size = new Vector2(1.15f, 1.6f);
            counterCollider.isTrigger = true;
            WorldActionStationView station = counter.AddComponent<WorldActionStationView>();
            station.Configure("citizen_noaharan", action, targetPlaceId, prompt, infoPanel);
            InteractableView interactable = counter.AddComponent<InteractableView>();
            interactable.Configure(station.GetPrompt, station.Interact, 125, 2.0f);
            CreateWorldLabel(counter.transform, prompt.Replace("E: ", ""), new Vector3(0f, -0.65f, -0.05f));
            CreateQuad("ShelfLeft", new Vector3(-3.7f, 0.75f, 0.45f), new Vector3(0.65f, 2.1f, 1f), accent * 0.75f);
            CreateQuad("ShelfRight", new Vector3(3.7f, 0.75f, 0.45f), new Vector3(0.65f, 2.1f, 1f), accent * 0.75f);
            CreateQuad("WarmLight", new Vector3(0f, 1.65f, 0.35f), new Vector3(1.2f, 0.28f, 1f), WarmLight);
        }

        private static void CreateStreetMarker(float x)
        {
            CreateQuad("RoadDash", new Vector3(x, -1.1f, 0.5f), new Vector3(0.9f, 0.08f, 1f), new Color(0.78f, 0.70f, 0.48f));
        }

        private static Camera CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 7.5f;
            camera.backgroundColor = new Color(0.12f, 0.12f, 0.14f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            return camera;
        }

        private static GameObject CreateQuad(string name, Vector3 position, Vector3 scale, Color color)
        {
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = name;
            quad.transform.position = position;
            quad.transform.localScale = scale;
            Remove3DCollider(quad);

            Renderer renderer = quad.GetComponent<Renderer>();
            renderer.material = CreateFlatMaterial(color);
            return quad;
        }

        private static TextMesh CreateWorldLabel(Transform parent, string text, Vector3 localPosition)
        {
            var labelObject = new GameObject($"{parent.name}Label");
            labelObject.transform.SetParent(parent);
            labelObject.transform.localPosition = localPosition;
            labelObject.transform.localScale = Vector3.one * 0.22f;

            TextMesh label = labelObject.AddComponent<TextMesh>();
            label.text = text;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.28f;
            label.fontSize = 18;
            label.color = Color.white;
            return label;
        }

        private static Material CreateFlatMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null) {
                shader = Shader.Find("Sprites/Default");
            }

            if (shader == null) {
                shader = Shader.Find("Unlit/Color");
            }

            return new Material(shader) {
                color = color
            };
        }

        private static void Remove3DCollider(GameObject target)
        {
            Collider collider = target.GetComponent<Collider>();
            if (collider != null) {
                Object.DestroyImmediate(collider);
            }
        }
    }
}
