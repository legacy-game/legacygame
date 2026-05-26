using Legacy.Commands;
using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldActionStationView : MonoBehaviour
    {
        [SerializeField] private string _actorCitizenId = "citizen_noaharan";
        [SerializeField] private WorldActionKind _action = WorldActionKind.ServeCustomer;
        [SerializeField] private string _targetPlaceId = "place_linden_cafe_interior";
        [SerializeField] private string _prompt = "E: do action";
        [SerializeField] private PropertyInfoPanel _infoPanel;

        public void Configure(
            string actorCitizenId,
            WorldActionKind action,
            string targetPlaceId,
            string prompt,
            PropertyInfoPanel infoPanel)
        {
            _actorCitizenId = actorCitizenId;
            _action = action;
            _targetPlaceId = targetPlaceId;
            _prompt = prompt;
            _infoPanel = infoPanel;
        }

        private void Awake()
        {
            InteractableView interactable = GetComponent<InteractableView>();
            if (interactable == null) {
                interactable = gameObject.AddComponent<InteractableView>();
            }

            interactable.Configure(GetPrompt, Interact, 125, 2.0f);
        }

        public string GetPrompt()
        {
            return _prompt;
        }

        public void Interact()
        {
            if (WorldBootstrap.Runtime == null || _infoPanel == null) {
                return;
            }

            _infoPanel.ShowJobMiniGame(
                new WorldEntityId(_actorCitizenId),
                _action,
                new WorldEntityId(_targetPlaceId));
        }
    }
}
