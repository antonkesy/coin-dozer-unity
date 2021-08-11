using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RayCastInput(Input.mousePosition);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount <= 0) return;

        foreach (Touch touch in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // ui touched
                return;
            }
        }

        RayCastInput(Input.touches.First(touch => touch.phase == TouchPhase.Began).position);
    }

    private void RayCastInput(Vector3 clickPosition)
    {
        var ray = _main.ScreenPointToRay(clickPosition);

        if (Physics.Raycast(ray, out var hit))
        {
            if (XInRange(hit.point.x) && ZInRange(hit.point.z))
            {
                coinManager.ClickPos(hit.point);
            }
        }
    }


    private bool XInRange(float x)
    {
        return InRange(-coinSpawnWidth, coinSpawnWidth, x);
    }

    private bool ZInRange(float z)
    {
        return InRange(zMin, zMax, z);
    }

    private static bool InRange(float min, float max, float value)
    {
        return value >= min && value <= max;
    }
}