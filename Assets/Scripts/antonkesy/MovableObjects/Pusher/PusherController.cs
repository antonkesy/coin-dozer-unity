using UnityEngine;

namespace antonkesy.MovableObjects.Pusher
{
    public class PusherController : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0F;
        [SerializeField] private Transform targetTransformForward;
        [SerializeField] private Transform targetTransformBackwards;

        private bool _isForward = true;
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 position = Vector3.MoveTowards(_rb.position,
                _isForward ? targetTransformForward.position : targetTransformBackwards.position,
                speed * Time.fixedDeltaTime);

            _rb.MovePosition(position);

            if (Mathf.Abs(_rb.position.z -
                          (_isForward ? targetTransformForward.position.z : targetTransformBackwards.position.z))
                <= 1.0)
            {
                _isForward = !_isForward;
            }
        }
    }
}