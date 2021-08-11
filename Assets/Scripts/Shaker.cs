using UnityEngine;

public class Shaker : MonoBehaviour
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

        shakeAnimation.Play();
        _maxShakeTime = shakeTime;
    }

    private void Update()
    {
        _timeShaked += Time.fixedDeltaTime;
        if (_timeShaked > _maxShakeTime)
        {
            ShakeReset();
        }
    }

    private void ShakeReset()
    {
        _isShaking = false;
        _timeShaked = 0F;
    }
}