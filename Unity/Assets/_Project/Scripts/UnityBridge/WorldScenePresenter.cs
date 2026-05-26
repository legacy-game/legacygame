using System.Collections.Generic;
using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldScenePresenter : MonoBehaviour
    {
        private static readonly WorldEntityId PlayerCitizenId = new("citizen_noaharan");

        [SerializeField] private string _sceneId = WorldSceneIds.ExteriorSceneId;
        [SerializeField] private PropertyInfoPanel _infoPanel;

        private readonly List<GameObject> _spawned = new();

        public void SetSceneId(string sceneId)
        {
            _sceneId = sceneId;
        }

        public void SetInfoPanel(PropertyInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
        }

        private void Start()
        {
            Rebuild();

            if (WorldBootstrap.Runtime != null) {
                WorldBootstrap.Runtime.CommandExecuted += OnWorldCommandExecuted;
                WorldBootstrap.Runtime.StateReplaced += OnWorldStateReplaced;
            }
        }

        private void OnDestroy()
        {
            if (WorldBootstrap.Runtime != null) {
                WorldBootstrap.Runtime.CommandExecuted -= OnWorldCommandExecuted;
                WorldBootstrap.Runtime.StateReplaced -= OnWorldStateReplaced;
            }
        }

        private void OnWorldCommandExecuted(Commands.WorldCommandResult result)
        {
            if (result.Succeeded) {
                Rebuild();
            }
        }

        private void OnWorldStateReplaced(WorldState state)
        {
            Rebuild();
        }

        private void Rebuild()
        {
            Clear();

            if (WorldBootstrap.Runtime == null) {
                return;
            }

            var sceneId = new WorldEntityId(_sceneId);
            int index = 0;

            IReadOnlyList<WorldEntityId> buildingIds = WorldBootstrap.Runtime.State.GetBuildingIdsInScene(sceneId);
            for (int i = 0; i < buildingIds.Count; i++) {
                if (WorldBootstrap.Runtime.State.TryGetBuilding(buildingIds[i], out BuildingState building)) {
                    SpawnBuilding(building, index);
                    index++;
                }
            }

            IReadOnlyList<WorldEntityId> citizenIds = WorldBootstrap.Runtime.State.GetCitizenIdsInScene(sceneId);
            for (int i = 0; i < citizenIds.Count; i++) {
                if (WorldBootstrap.Runtime.State.TryGetCitizen(citizenIds[i], out CitizenState citizen)) {
                    if (citizen.Id == PlayerCitizenId) {
                        continue;
                    }

                    SpawnCitizen(citizen, i);
                }
            }
        }

        private void SpawnBuilding(BuildingState building, int index)
        {
            bool isInteriorView = building.InteriorSceneId == new WorldEntityId(_sceneId);
            Vector3 position = isInteriorView
                ? Vector3.zero
                : ExteriorBuildingPosition(index);

            Color color = isInteriorView
                ? new Color(0.30f, 0.22f, 0.17f)
                : ColorForIndex(index);
            GameObject propertyObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            propertyObject.name = $"{building.DisplayName}_View";
            propertyObject.transform.SetParent(transform);
            propertyObject.transform.position = new Vector3(position.x, position.y, 0f);
            propertyObject.transform.localScale = isInteriorView
                ? new Vector3(4.8f, 2.6f, 1f)
                : new Vector3(2.25f, 1.55f, 1f);
            Remove3DCollider(propertyObject);

            BoxCollider2D collider = propertyObject.GetComponent<BoxCollider2D>();
            if (collider == null) {
                collider = propertyObject.AddComponent<BoxCollider2D>();
            }

            if (collider != null) {
                collider.size = new Vector2(2.2f, 1.4f);
            }

            Renderer renderer = propertyObject.GetComponent<Renderer>();
            renderer.material = CreateFlatMaterial(color);

            BuildingView buildingView = propertyObject.AddComponent<BuildingView>();
            buildingView.SetBuildingId(building.Id.Value);
            buildingView.SetInfoPanel(_infoPanel);
            buildingView.SetLabel(CreateLabel(propertyObject.transform, new Vector3(0f, -1.1f, -0.05f), building.DisplayName));
            InteractableView interactable = propertyObject.AddComponent<InteractableView>();
            interactable.Configure(buildingView.GetPrompt, buildingView.Interact);

            _spawned.Add(propertyObject);
        }

        private void SpawnCitizen(CitizenState citizen, int index)
        {
            Vector3 position = GridToWorld(citizen.CurrentCoord, index);
            position += new Vector3(0.55f, 0.25f + index * 0.15f, -0.25f);

            GameObject citizenObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            citizenObject.name = $"{citizen.DisplayName}_Citizen";
            citizenObject.transform.SetParent(transform);
            citizenObject.transform.position = position;
            citizenObject.transform.localScale = new Vector3(0.55f, 0.85f, 1f);
            Remove3DCollider(citizenObject);

            Renderer renderer = citizenObject.GetComponent<Renderer>();
            renderer.material = CreateFlatMaterial(ColorForCitizen(citizen));

            CitizenView citizenView = citizenObject.AddComponent<CitizenView>();
            citizenView.SetCitizenId(citizen.Id.Value);
            citizenView.SetInfoPanel(_infoPanel);
            citizenView.SetLabel(CreateLabel(citizenObject.transform, new Vector3(0f, -0.8f, -0.05f), citizen.DisplayName));
            InteractableView interactable = citizenObject.AddComponent<InteractableView>();
            interactable.Configure(citizenView.GetPrompt, citizenView.Interact);

            _spawned.Add(citizenObject);
        }

        private Color ColorForIndex(int index)
        {
            return index % 2 == 0
                ? new Color(0.38f, 0.25f, 0.18f)
                : new Color(0.18f, 0.30f, 0.38f);
        }

        private Color ColorForCitizen(CitizenState citizen)
        {
            return citizen.Activity == CitizenActivityState.Working
                ? new Color(0.35f, 0.60f, 0.45f)
                : new Color(0.50f, 0.45f, 0.70f);
        }

        private Vector3 GridToWorld(GridCoord coord, int fallbackIndex)
        {
            if (_sceneId != WorldSceneIds.ExteriorSceneId) {
                return new Vector3(-1.2f + fallbackIndex * 1.2f, -0.35f, -0.1f);
            }

            return new Vector3((coord.X - 15f) * 0.38f, (coord.Y - 7f) * 0.32f, -0.1f);
        }

        private Vector3 ExteriorBuildingPosition(int index)
        {
            return index switch {
                0 => new Vector3(-4.6f, 0.75f, -0.1f),
                1 => new Vector3(-1.3f, 0.75f, -0.1f),
                2 => new Vector3(2.3f, 0.75f, -0.1f),
                _ => new Vector3(5.2f + (index - 3) * 2.4f, 0.75f, -0.1f)
            };
        }

        private TextMesh CreateLabel(Transform parent, Vector3 localPosition, string text)
        {
            var labelObject = new GameObject("Label");
            labelObject.transform.SetParent(parent);
            labelObject.transform.localPosition = localPosition;
            labelObject.transform.localScale = Vector3.one * 0.18f;

            TextMesh label = labelObject.AddComponent<TextMesh>();
            label.text = text;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.3f;
            label.fontSize = 18;
            label.color = Color.white;
            return label;
        }

        private Material CreateFlatMaterial(Color color)
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

        private void Remove3DCollider(GameObject target)
        {
            Collider collider = target.GetComponent<Collider>();
            if (collider != null) {
                DestroyImmediate(collider);
            }
        }

        private void Clear()
        {
            for (int i = 0; i < _spawned.Count; i++) {
                if (_spawned[i] != null) {
                    Destroy(_spawned[i]);
                }
            }

            _spawned.Clear();
        }
    }
}
