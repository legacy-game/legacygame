using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _followSharpness = 8f;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target == null) {
                return;
            }

            Vector3 targetPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1f - Mathf.Exp(-_followSharpness * UnityEngine.Time.deltaTime));
        }
    }
}
