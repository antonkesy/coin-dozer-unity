using System.Collections.Generic;
using UnityEngine;


public class CoinRemoveManager : MonoBehaviour
{
    private class CoinDeleteData
    {
        public Coin Coin;
        public float DeletionTime;

        internal CoinDeleteData(Coin coin)
        {
            Coin = coin;
            DeletionTime = Time.time + 10F;
        }
    }

    private readonly Queue<CoinDeleteData> RemoveCoinQue = new Queue<CoinDeleteData>();

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
        RemoveCoinQue.Enqueue(new CoinDeleteData(coin));
    }

    private void CheckRemoveCoinQue()
    {
        if (RemoveCoinQue.Count > 0 && RemoveCoinQue.Peek().DeletionTime < Time.time)
        {
            _smartCoinList.Remove(RemoveCoinQue.Dequeue().Coin);
        }
    }
}