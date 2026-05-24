using Legacy.Commands;
using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class RegistryStationView : MonoBehaviour
    {
        [SerializeField] private string _citizenId = "citizen_noaharan";
        [SerializeField] private string _registryPlaceId = "place_linden_street";
        [SerializeField] private string _startingResidencePlaceId = "place_linden_street";
        [SerializeField] private string _firstGoalTargetPlaceId = "place_linden_cafe_interior";
        [SerializeField] private PropertyInfoPanel _infoPanel;

        public void Configure(PropertyInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
        }

        private void Awake()
        {
            InteractableView interactable = GetComponent<InteractableView>();
            if (interactable == null) {
                interactable = gameObject.AddComponent<InteractableView>();
            }

            interactable.Configure(GetPrompt, Interact, 75, 1.8f);
        }

        public string GetPrompt()
        {
            if (WorldBootstrap.Runtime == null) {
                return "Civic desk: press E to register";
            }

            var citizenId = new WorldEntityId(_citizenId);
            if (WorldBootstrap.Runtime.State.TryGetCitizenRegistration(citizenId, out CitizenRegistrationState _)) {
                return "Civic desk: registered. Visit Linden Cafe.";
            }

            return "Civic desk: press E to register";
        }

        public void Interact()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldCommandResult result = WorldBootstrap.Runtime.Execute(new RegisterCitizenCommand(
                new WorldEntityId(_citizenId),
                new WorldEntityId(_registryPlaceId),
                new WorldEntityId(_startingResidencePlaceId),
                new WorldEntityId(_firstGoalTargetPlaceId),
                new GridCoord(4, 3)));

            if (_infoPanel != null) {
                _infoPanel.Show(result.Message);
            }
        }
    }
}
