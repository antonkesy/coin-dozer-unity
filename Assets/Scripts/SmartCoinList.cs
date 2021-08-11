using System.Collections.Generic;
using UnityEngine;

internal class SmartCoinList
{
    private readonly CoinManager _coinManager;
    private readonly LinkedList<int> _unusedCoinIndex;
    internal List<Coin> Coins { get; } //TODO own implementation of list with add returns index

    internal SmartCoinList(CoinManager coinManager, int size = 50)
    {
        _coinManager = coinManager;
        Coins = new List<Coin>(size);
        _unusedCoinIndex = new LinkedList<int>();

        BuildNewLinkedList(size);
        FillPoolWithCoins(size);
    }

    private void FillPoolWithCoins(int size)
    {
        for (var i = 0; i < size; ++i)
        {
            var newCoin = _coinManager.SpawnCoin(Vector3.zero);
            newCoin.gameObject.SetActive(false);
            newCoin.Index = i;
            Coins.Add(newCoin);
        }
    }

    private void BuildNewLinkedList(int size)
    {
        for (var i = 0; i < size; ++i)
        {
            _unusedCoinIndex.AddLast(i);
        }
    }

    internal void Remove(Coin coin)
    {
        coin.SetUnused();
        _unusedCoinIndex.AddLast(coin.Index);
    }

    internal void Add(GameSaver.SaveData.CoinData coinData)
    {
        SetCoinFromCoinData(coinData);
    }

    //TODO no value differentiation
    internal Coin Add(Vector3 position)
    {
        var unusedCoinSpot = GetUnusedCoinIndex();
        if (unusedCoinSpot >= 0)
        {
            var coin = Coins[unusedCoinSpot];
            coin.transform.localPosition = position;
            coin.gameObject.SetActive(true);
            return coin;
        }

        //if no free coins available
        //!! ASSUMING add always to end of list
        //TODO fix or own implementation
        var count = Coins.Count;
        var newCoin = _coinManager.SpawnCoin(position);
        newCoin.Index = count;
        Coins.Add(newCoin);
        return newCoin;
    }

    private int GetUnusedCoinIndex()
    {
        var index = -1;
        var firsUnusedCoin = _unusedCoinIndex.First;
        if (firsUnusedCoin != null)
        {
            index = _unusedCoinIndex.First.Value;
            _unusedCoinIndex.RemoveFirst();
        }

        return index;
    }

    private void SetCoinFromCoinData(GameSaver.SaveData.CoinData coinData)
    {
        var coin = Add(coinData.position);
        coin.gameObject.transform.rotation = coinData.rotation;
        coin.Rb.velocity = coinData.velocity;
        coin.Rb.angularVelocity = coinData.angularVelocity;
        coin.value = coinData.value;
    }
}