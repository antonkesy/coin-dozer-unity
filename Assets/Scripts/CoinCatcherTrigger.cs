using UnityEngine;

public class CoinCatcherTrigger : MonoBehaviour
{
    private bool _isWin;
    private CoinCatcher _coinCatcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            _coinCatcher.CallbackCoinCatcherTrigger(_isWin, other.GetComponent<Coin>());
        }
    }


    public void Setup(CoinCatcher coinCatcher, bool isWin)
    {
        _coinCatcher = coinCatcher;
        _isWin = isWin;
    }
}