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
                    var position = hit.point;
                    position.y = coinSpawnHeight;
                    AddCoin(position);
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
        var newCoin = Instantiate(coinPrefab, transform);
        newCoin.transform.localPosition = position;
        return newCoin;
    }

    //TODO refactor to add coin to always call this instead of raw? performance lost
    private GameObject AddCoin(Vector3 position)
    {
        //O(n)
        //TODO add flag when full
        //TODO linked list for free spots
        foreach (var coin in _coinPool)
        {
            if (!coin.activeSelf)
            {
                coin.transform.localPosition = position;
                coin.SetActive(true);
                return coin;
            }
        }

        //else
        var newCoin = SpawnCoin(position);
        _coinPool.Add(newCoin);
        return newCoin;
    }

    private void SetCoinsNewStartPos()
    {
        var newStartRows = 6;
        var newStartColumns = 6;

        var smallRowCount = 4;
        var spacer = .1f;
        var coinRadius = coinPrefab.transform.localScale.x;
        var zPosCoin = 3.5F; //z start pos
        var xPosCoin = coinRadius * smallRowCount / 2 - coinRadius / 2 + spacer * smallRowCount / 2;
        var spacePerCoin = coinRadius + spacer;
        //small row
        SetRowCoins(xPosCoin, zPosCoin, spacePerCoin, smallRowCount);
        //full rows
        for (var rows = 0; rows < newStartRows; ++rows)
        {
            xPosCoin = coinRadius * newStartColumns / 2 - coinRadius / 2 + spacer * newStartColumns / 2;
            zPosCoin += coinRadius + spacer;
            SetRowCoins(xPosCoin, zPosCoin, spacePerCoin, newStartColumns);
        }
    }

    private void SetRowCoins(float xPosStart, float zPos, float spacePerCoin, int amount)
    {
        for (var column = 0; column < amount; ++column)
        {
            var coin = AddCoin(new Vector3(xPosStart, 0, zPos));
            coin.SetActive(true);
            xPosStart -= spacePerCoin;
        }
    }
}