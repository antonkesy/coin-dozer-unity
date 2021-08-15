using UnityEngine;

namespace antonkesy.MovableObjects.Shaker
{
    public class ShakerController : MonoBehaviour
    {
        [SerializeField] private Animation shakeAnimation;

        private bool _isShaking;

        private float _timeShaked;

        private int _maxShakeTime;

        private bool _isLeftShake;
        private float _sideShakeTime;

        internal void StartShaking(int shakeTime)
        {
            if (_isShaking)
            {
                return;
            }

            _isShaking = true;
            _maxShakeTime = shakeTime;
        }

        private void Update()
        {
            if (_timeShaked > _maxShakeTime)
            {
                ShakeReset();
            }

            _timeShaked += Time.fixedDeltaTime;

            if (_isShaking)
            {
                shakeAnimation.Play();
            }
        }

        private void ShakeReset()
        {
            _isShaking = false;
            _timeShaked = 0F;
        }
    }
}