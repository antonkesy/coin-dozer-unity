using System.Collections.Generic;
using System.Linq;
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


    private void Awake()
    {
        _main = Camera.main;
        FillPoolWithCoins();
    }

    internal void StartCall(bool loadSaveData, GameSaver.SaveData saveData)
    {
        if (loadSaveData)
        {
            SetCoinsFromSavedPosition(saveData);
        }
        else
        {
            SetCoinsNewStartPos();
        }
    }

    private void SetCoinsFromSavedPosition(GameSaver.SaveData savedData)
    {
        foreach (var coin in savedData.coins)
        {
            AddCoin(coin);
        }
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

    private GameObject AddCoin(GameSaver.SaveData.CoinData coinData)
    {
        var coin = AddCoin(coinData.position);
        coin.transform.rotation = coinData.rotation;
        var rb=coin.GetComponent<Rigidbody>();
        rb.velocity= coinData.velocity;
        rb.angularVelocity = coinData.angularVelocity;
        //todo set coin value
        return coin;
    }

    //TODO refactor to add coin to always call this instead of raw? performance lost
    //TODO no value differentiation
    private GameObject AddCoin(Vector3 position)
    {
        //O(n)
        //TODO add flag when full
        //TODO linked list for free spots
        foreach (var coin in _coinPool.Where(coin => !coin.activeSelf))
        {
            coin.transform.localPosition = position;
            coin.SetActive(true);
            return coin;
        }

        //else
        var newCoin = SpawnCoin(position);
        _coinPool.Add(newCoin);
        return newCoin;
    }

    private void SetCoinsNewStartPos()
    {
        const int newStartRows = 6;
        const int newStartColumns = 6;

        const int smallRowCount = 4;
        const float spacer = .1f;
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

    internal List<GameObject> GetUsedCoins()
    {
        return _coinPool.Where(coin => coin.activeSelf).ToList();
    }
}