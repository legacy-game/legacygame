using Legacy.Commands;
using Legacy.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Legacy.UnityBridge
{
    public sealed class PropertyInteractionBridge : MonoBehaviour
    {
        [SerializeField] private string _playerCitizenId = "citizen_noaharan";
        [SerializeField] private Camera _camera;
        [SerializeField] private PropertyInfoPanel _infoPanel;

        private void Awake()
        {
            if (_camera == null) {
                _camera = Camera.main;
            }
        }

        public void SetInfoPanel(PropertyInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
        }

        public void SetCamera(Camera targetCamera)
        {
            _camera = targetCamera;
        }

        private void Update()
        {
            if (WorldBootstrap.Runtime == null || Mouse.current == null || _camera == null) {
                return;
            }

            if (!Mouse.current.leftButton.wasPressedThisFrame) {
                return;
            }

            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPosition);

            if (hit == null || !hit.TryGetComponent(out BuildingView building)) {
                return;
            }

            WorldCommandResult result = WorldBootstrap.Runtime.Execute(new InspectBuildingCommand(
                building.BuildingId,
                new WorldEntityId(_playerCitizenId)));

            if (_infoPanel != null) {
                _infoPanel.Show(result.Message);
            }
        }
    }
}
