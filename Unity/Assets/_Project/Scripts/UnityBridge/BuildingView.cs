using Legacy.World;
using UnityEngine;
using Legacy.Commands;

namespace Legacy.UnityBridge
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class BuildingView : MonoBehaviour
    {
        [SerializeField] private string _buildingId = "building_linden_cafe";
        [SerializeField] private TextMesh _label;
        [SerializeField] private PropertyInfoPanel _infoPanel;
        [SerializeField] private string _playerCitizenId = "citizen_noaharan";

        public WorldEntityId BuildingId => new WorldEntityId(_buildingId);
        public string CurrentDisplayText { get; private set; }

        public void SetInfoPanel(PropertyInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
        }

        private void Awake()
        {
            Refresh();
        }

        private void Start()
        {
            Refresh();
            SubscribeToRuntime();
        }

        private void OnEnable()
        {
            SubscribeToRuntime();
        }

        private void OnDisable()
        {
            if (WorldBootstrap.Runtime != null) {
                WorldBootstrap.Runtime.CommandExecuted -= OnWorldCommandExecuted;
                WorldBootstrap.Runtime.StateReplaced -= OnWorldStateReplaced;
            }
        }

        private void SubscribeToRuntime()
        {
            if (WorldBootstrap.Runtime != null) {
                WorldBootstrap.Runtime.CommandExecuted -= OnWorldCommandExecuted;
                WorldBootstrap.Runtime.CommandExecuted += OnWorldCommandExecuted;
                WorldBootstrap.Runtime.StateReplaced -= OnWorldStateReplaced;
                WorldBootstrap.Runtime.StateReplaced += OnWorldStateReplaced;
            }
        }

        public void SetLabel(TextMesh label)
        {
            _label = label;
            Refresh();
        }

        public void SetBuildingId(string buildingId)
        {
            _buildingId = buildingId;
            Refresh();
        }

        public void Refresh()
        {
            CurrentDisplayText = GetDisplayText();

            if (_label != null) {
                _label.text = CurrentDisplayText;
            }
        }

        public string GetDisplayText()
        {
            if (WorldBootstrap.Runtime == null) {
                return "World runtime not initialized.";
            }

            if (!WorldBootstrap.Runtime.State.TryGetBuilding(BuildingId, out BuildingState building)) {
                return $"Missing building: {_buildingId}";
            }

            string owner = WorldBootstrap.Runtime.State.TryGetCitizen(building.OwnerCitizenId, out CitizenState citizen)
                ? citizen.DisplayName
                : building.OwnerCitizenId.ToString();

            return $"{building.DisplayName}\nOwner: {owner}";
        }

        public string GetPrompt()
        {
            if (WorldBootstrap.Runtime == null || !WorldBootstrap.Runtime.State.TryGetBuilding(BuildingId, out BuildingState building)) {
                return "E: inspect building";
            }

            return $"E: inspect {building.DisplayName}";
        }

        public void Interact()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldCommandResult result = WorldBootstrap.Runtime.Execute(new InspectBuildingCommand(
                BuildingId,
                new WorldEntityId(_playerCitizenId)));

            if (_infoPanel != null) {
                _infoPanel.Show(result.Message);
            }
        }

        private void OnWorldCommandExecuted(Commands.WorldCommandResult result)
        {
            if (!result.Succeeded) {
                return;
            }

            for (int i = 0; i < result.ChangedEntityIds.Count; i++) {
                if (result.ChangedEntityIds[i] == BuildingId) {
                    Refresh();
                    return;
                }
            }
        }

        private void OnWorldStateReplaced(World.WorldState state)
        {
            Refresh();
        }
    }
}
