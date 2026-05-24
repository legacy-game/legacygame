using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class CitizenView : MonoBehaviour
    {
        [SerializeField] private string _citizenId;
        [SerializeField] private TextMesh _label;
        [SerializeField] private PropertyInfoPanel _infoPanel;

        public WorldEntityId CitizenId => new WorldEntityId(_citizenId);

        public void SetInfoPanel(PropertyInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
        }

        public void SetCitizenId(string citizenId)
        {
            _citizenId = citizenId;
            Refresh();
        }

        public void SetLabel(TextMesh label)
        {
            _label = label;
            Refresh();
        }

        public void Refresh()
        {
            if (_label == null || WorldBootstrap.Runtime == null) {
                return;
            }

            if (!WorldBootstrap.Runtime.State.TryGetCitizen(CitizenId, out CitizenState citizen)) {
                _label.text = $"Missing citizen\n{_citizenId}";
                return;
            }

            _label.text = $"{citizen.DisplayName}\n{citizen.Activity}";
        }

        public string GetPrompt()
        {
            if (WorldBootstrap.Runtime == null || !WorldBootstrap.Runtime.State.TryGetCitizen(CitizenId, out CitizenState citizen)) {
                return "E: inspect citizen";
            }

            return $"E: check on {citizen.DisplayName}";
        }

        public void Interact()
        {
            if (WorldBootstrap.Runtime == null || _infoPanel == null) {
                return;
            }

            if (!WorldBootstrap.Runtime.State.TryGetCitizen(CitizenId, out CitizenState citizen)) {
                _infoPanel.Show("Citizen not found.");
                return;
            }

            string place = WorldBootstrap.Runtime.State.TryGetPlace(citizen.CurrentPlaceId, out PlaceState placeState)
                ? placeState.DisplayName
                : citizen.CurrentPlaceId.ToString();
            string intent = string.IsNullOrWhiteSpace(citizen.Routine.CurrentIntent)
                ? "No current intent"
                : citizen.Routine.CurrentIntent;
            _infoPanel.Show($"{citizen.DisplayName}\n{citizen.Activity} at {place}\n{intent}");
        }
    }
}
