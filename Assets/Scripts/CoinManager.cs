using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int coinsInPool = 50;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnHeight;

    private SmartCoinList _coins;

    [SerializeField] private CoinRemoveManager coinRemoveManager;

    private void Awake()
    {
        _coins = new SmartCoinList(this, coinsInPool);
        coinRemoveManager.SetUp(_coins);
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

    public void ClickPos(Vector3 hitInfoPoint)
    {
        hitInfoPoint.y = coinSpawnHeight;
        _coins.Add(hitInfoPoint);
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
        coinRemoveManager.RemoveCoin(coin);
    }
}