using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private List<GameObject> _coinPool;
    [SerializeField] private Vector3 coinPoolPos;
    [SerializeField] private int coinsInPool = 50;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnHeight;

    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;
    private Camera _main;


    private void Start()
    {
        _main = Camera.main;
        FillPoolWithCoins();

        //
        SetCoinsNewStartPos();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.point.x >= xMin && hit.point.x <= xMax && hit.point.y >= yMin && hit.point.y <= yMax)
                {
                    UserAddCoin(hit.point);
                }
            }
        }
    }

    private void FillPoolWithCoins()
    {
        _coinPool = new List<GameObject>
        {
            Capacity = coinsInPool
        };
        for (var i = 0; i < coinsInPool; ++i)
        {
            var newCoin = SpawnCoin(coinPoolPos);
            newCoin.SetActive(false);
            _coinPool.Add(newCoin);
        }
    }

    private GameObject SpawnCoin(Vector3 position)
    {
        position.y = coinSpawnHeight;
        var newCoin = Instantiate(coinPrefab, transform);
        newCoin.transform.localPosition = position;
        return newCoin;
    }

    private void UserAddCoin(Vector3 position)
    {
        position.y = coinSpawnHeight;
        //O(n)
        //TODO add flag when full
        foreach (var t in _coinPool)
        {
            if (!t.activeSelf)
            {
                t.transform.localPosition = position;
                t.SetActive(true);
                return;
            }
        }

        //else
        _coinPool.Add(SpawnCoin(position));
    }

    private void SetCoinsNewStartPos()
    {
        var newStartRows = 6;
        var newStartColumns = 6;
        var smallRowCount = 4;
        var spacer = .1f;
        var coinRadius = coinPrefab.transform.localScale.x;
        var zPosCoin = 3.5F; //y start pos
        var xPosCoin = coinRadius * smallRowCount / 2 - coinRadius / 2 + spacer * smallRowCount / 2;
        //small row
        for (var i = 0; i < smallRowCount; ++i)
        {
            _coinPool[i].transform.position = new Vector3(xPosCoin, 0, zPosCoin);
            _coinPool[i].SetActive(true);
            xPosCoin -= coinRadius + spacer;
        }

        //full rows
        for (var rows = 0; rows < newStartRows; ++rows)
        {
            xPosCoin = coinRadius * newStartColumns / 2 - coinRadius / 2 + spacer * newStartColumns / 2;
            zPosCoin += coinRadius + spacer;
            for (var column = 0; column < newStartColumns; ++column)
            {
                var index = newStartColumns * rows + column + smallRowCount;
                _coinPool[index].transform.position = new Vector3(xPosCoin, 0, zPosCoin);
                _coinPool[index].SetActive(true);
                xPosCoin -= coinRadius + spacer;
            }
        }
    }
}