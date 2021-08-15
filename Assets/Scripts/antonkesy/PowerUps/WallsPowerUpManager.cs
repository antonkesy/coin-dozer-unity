using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace antonkesy.PowerUps
{
    public class WallsPowerUpManager : MonoBehaviour, IPowerUp
    {
        [SerializeField] private float startY = -10F;
        [SerializeField] private float timeUp;

        [SerializeField] private float maxTime;

        private bool _isActive;

        private Vector3 _startPosition;


        private void Start()
        {
            _startPosition = Vector3.up * startY;
            transform.position = _startPosition;
        }


        private void Update()
        {
            transform.position = Vector3.LerpUnclamped(transform.position, _isActive ? Vector3.zero : _startPosition,
                3F * Time.deltaTime);

            UpdateActiveTimer();
        }

        public void Activate()
        {
            if (_isActive)
            {
                timeUp -= maxTime;
            }

            _isActive = true;
        }

        private void UpdateActiveTimer()
        {
            if (!_isActive) return;

            timeUp += Time.deltaTime;
            if (timeUp > maxTime)
            {
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            timeUp = 0;
            _isActive = false;
        }
    }
}