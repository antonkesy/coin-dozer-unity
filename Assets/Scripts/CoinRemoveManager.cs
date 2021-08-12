using System.Collections.Generic;
using UnityEngine;


public class CoinRemoveManager : MonoBehaviour
{
    private class CoinDeleteData
    {
        public readonly Coin Coin;
        public readonly float DeletionTime;

        internal CoinDeleteData(Coin coin)
        {
            Coin = coin;
            DeletionTime = Time.time + 10F;
        }
    }

    private readonly Queue<CoinDeleteData> _removeCoinQue = new Queue<CoinDeleteData>();

    private SmartCoinList _smartCoinList;


    internal void SetUp(SmartCoinList smartCoinList)
    {
        _smartCoinList = smartCoinList;
    }


    private void Update()
    {
        CheckRemoveCoinQue();
    }


    public void RemoveCoin(Coin coin)
    {
        _removeCoinQue.Enqueue(new CoinDeleteData(coin));
    }

    private void CheckRemoveCoinQue()
    {
        if (_removeCoinQue.Count > 0 && _removeCoinQue.Peek().DeletionTime < Time.time)
        {
            _smartCoinList.Remove(_removeCoinQue.Dequeue().Coin);
        }
    }
}