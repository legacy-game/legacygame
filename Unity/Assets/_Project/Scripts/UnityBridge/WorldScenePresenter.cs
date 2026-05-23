using System.Collections.Generic;
using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldScenePresenter : MonoBehaviour
    {
        [SerializeField] private string _sceneId = WorldSceneIds.ExteriorSceneId;

        private readonly List<GameObject> _spawned = new();

        public void SetSceneId(string sceneId)
        {
            _sceneId = sceneId;
        }

        private void Start()
        {
            Rebuild();

            if (WorldBootstrap.Runtime != null) {
                WorldBootstrap.Runtime.StateReplaced += OnWorldStateReplaced;
            }
        }

        private void OnDestroy()
        {
            if (WorldBootstrap.Runtime != null) {
                WorldBootstrap.Runtime.StateReplaced -= OnWorldStateReplaced;
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
        }

        private void SpawnBuilding(BuildingState building, int index)
        {
            bool isInteriorView = building.InteriorSceneId == new WorldEntityId(_sceneId);
            Vector3 position = isInteriorView
                ? Vector3.zero
                : new Vector3(-2.25f + index * 4.5f, 0f, 0f);

            Color color = isInteriorView
                ? new Color(0.42f, 0.26f, 0.18f)
                : ColorForIndex(index);

            GameObject propertyObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            propertyObject.name = $"{building.DisplayName}_View";
            propertyObject.transform.SetParent(transform);
            propertyObject.transform.position = position;
            propertyObject.transform.localScale = new Vector3(3.5f, 2f, 0.2f);

            BoxCollider boxCollider = propertyObject.GetComponent<BoxCollider>();
            if (boxCollider != null) {
                DestroyImmediate(boxCollider);
            }

            BoxCollider2D collider = propertyObject.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;

            Renderer renderer = propertyObject.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit")) {
                color = color
            };

            BuildingView buildingView = propertyObject.AddComponent<BuildingView>();
            buildingView.SetBuildingId(building.Id.Value);
            buildingView.SetLabel(CreateBuildingLabel(propertyObject.transform));

            _spawned.Add(propertyObject);
        }

        private TextMesh CreateBuildingLabel(Transform parent)
        {
            GameObject labelObject = new GameObject("OwnershipLabel");
            labelObject.transform.SetParent(parent);
            labelObject.transform.localPosition = new Vector3(-0.45f, 1.3f, -0.2f);
            labelObject.transform.localScale = Vector3.one * 0.18f;

            TextMesh label = labelObject.AddComponent<TextMesh>();
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.fontSize = 24;
            label.color = Color.white;
            return label;
        }

        private Color ColorForIndex(int index)
        {
            return index % 2 == 0
                ? new Color(0.55f, 0.36f, 0.24f)
                : new Color(0.25f, 0.38f, 0.50f);
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
