using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CoinManager coinManager;

    private Camera _main;

    [SerializeField] private float coinSpawnWidth = 9;

    [SerializeField] private float zMin;

    [SerializeField] private float zMax;

    private void Start()
    {
        _main = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                Debug.Log(hit.transform.position);

                if (XInRange(hit.point.x) && ZInRange(hit.point.z))
                {
                    coinManager.ClickPos(hit.point);
                }
            }
        }
    }

    private bool XInRange(float x)
    {
        Debug.Log("X " + InRange(-coinSpawnWidth, coinSpawnWidth, x));
        return InRange(-coinSpawnWidth, coinSpawnWidth, x);
    }

    private bool ZInRange(float z)
    {
        Debug.Log("Z " + InRange(zMin, zMax, z));
        return InRange(zMin, zMax, z);
    }

    private bool InRange(float min, float max, float value)
    {
        return value >= min && value <= max;
    }
}