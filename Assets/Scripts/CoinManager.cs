using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnHeight;

    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;
    private Camera _main;

    // Start is called before the first frame update
    void Start()
    {
        _main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.point.x >= xMin && hit.point.x <= xMax && hit.point.y >= yMin && hit.point.y <= yMax)
                {
                    SpawnCoin(hit.point);
                }
            }
        }
    }

    private void SpawnCoin(Vector3 position)
    {
        position.y = coinSpawnHeight;
        var newCoin = Instantiate(coinPrefab, transform);
        newCoin.transform.position = position;
    }
}