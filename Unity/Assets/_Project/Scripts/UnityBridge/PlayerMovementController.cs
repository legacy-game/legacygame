using UnityEngine;
using UnityEngine.InputSystem;

namespace Legacy.UnityBridge
{
    public sealed class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 4.5f;
        [SerializeField] private Vector2 _minBounds = new(-8f, -5f);
        [SerializeField] private Vector2 _maxBounds = new(8f, 5f);
        [SerializeField] private PropertyInfoPanel _infoPanel;

        public void SetInfoPanel(PropertyInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) {
                return;
            }

            UpdateInteraction(keyboard);

            Vector2 input = Vector2.zero;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) {
                input.x -= 1f;
            }

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) {
                input.x += 1f;
            }

            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) {
                input.y -= 1f;
            }

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) {
                input.y += 1f;
            }

            if (input.sqrMagnitude > 1f) {
                input.Normalize();
            }

            Vector3 nextPosition = transform.position + (Vector3)(input * _moveSpeed * UnityEngine.Time.deltaTime);
            nextPosition.x = Mathf.Clamp(nextPosition.x, _minBounds.x, _maxBounds.x);
            nextPosition.y = Mathf.Clamp(nextPosition.y, _minBounds.y, _maxBounds.y);
            transform.position = nextPosition;
        }

        private void UpdateInteraction(Keyboard keyboard)
        {
            InteractableView nearest = FindNearestInteractable();
            if (nearest != null && _infoPanel != null) {
                _infoPanel.Show(nearest.Prompt);
            }

            if (nearest != null && keyboard.eKey.wasPressedThisFrame) {
                nearest.Interact();
            }
        }

        public void SetBounds(Vector2 minBounds, Vector2 maxBounds)
        {
            _minBounds = minBounds;
            _maxBounds = maxBounds;
        }

        private InteractableView FindNearestInteractable()
        {
            InteractableView[] interactables = FindObjectsByType<InteractableView>(FindObjectsSortMode.None);
            InteractableView nearest = null;
            float nearestDistanceSquared = float.MaxValue;
            int highestPriority = int.MinValue;

            for (int i = 0; i < interactables.Length; i++) {
                InteractableView interactable = interactables[i];
                float distanceSquared = (interactable.transform.position - transform.position).sqrMagnitude;
                float radiusSquared = interactable.Radius * interactable.Radius;
                if (distanceSquared > radiusSquared) {
                    continue;
                }

                if (interactable.Priority > highestPriority || (interactable.Priority == highestPriority && distanceSquared < nearestDistanceSquared)) {
                    nearest = interactable;
                    nearestDistanceSquared = distanceSquared;
                    highestPriority = interactable.Priority;
                }
            }

            return nearest;
        }
    }
}
