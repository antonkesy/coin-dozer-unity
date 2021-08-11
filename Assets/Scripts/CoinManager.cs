using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int coinsInPool = 50;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnHeight;

    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;
    private Camera _main;

    private SmartCoinList _coins;


    private void Awake()
    {
        _coins = new SmartCoinList(this, coinsInPool);
        _main = Camera.main;
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
            _coins.Add(coin);
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
                    _coins.Add(position);
                }
            }
        }
    }

    internal Coin SpawnCoin(Vector3 position)
    {
        var newCoin = Instantiate(coinPrefab, transform);
        newCoin.transform.localPosition = position;
        return newCoin.GetComponent<Coin>();
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
            var coin = _coins.Add(new Vector3(xPosStart, 0, zPos));
            coin.gameObject.SetActive(true);
            xPosStart -= spacePerCoin;
        }
    }

    internal List<GameObject> GetUsedCoins()
    {
        return _coins.Coins.Where(coin => coin.gameObject.activeSelf).Select(coin => coin.gameObject).ToList();
    }

    public void RemoveCoin(Coin coin)
    {
        _coins.Remove(coin);
    }
}