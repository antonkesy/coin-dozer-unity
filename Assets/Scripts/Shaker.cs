using System.Collections;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] private Transform shakeObjectsGroup;


    private bool _isShaking;

    internal void StartShaking(int shakes)
    {
        if (_isShaking)
        {
            return;
        }

        _isShaking = true;
        StartCoroutine(ShakeCoroutine(shakes));
    }

    private void FixedUpdate()
    {
        //todo move shaking here
    }

    private IEnumerator ShakeCoroutine(int shakes)
    {
#if UNITY_EDITOR
        Debug.Log("Start Shaking");
#endif
        for (var i = 0; i < shakes; ++i)
        {
            var side = i % 2 == 0;
            var rotation = new Vector3(side ? 0F : 3F, side ? 1F : -1F, side ? 2F : -2F);
            var position = new Vector3(0F, 0F, side ? 0.1F : -0.1F);
            SetShakeObjectRotation(rotation, position);
            yield return new WaitForSecondsRealtime(.3F);
        }

        SetShakeObjectRotation(Vector3.zero, Vector3.zero);

        _isShaking = false;
        yield return null;
    }

    private void SetShakeObjectRotation(Vector3 eulerRotation, Vector3 position)
    {
        //change rotation and position
        shakeObjectsGroup.position = position;
        shakeObjectsGroup.rotation = Quaternion.Euler(eulerRotation);
    }
}