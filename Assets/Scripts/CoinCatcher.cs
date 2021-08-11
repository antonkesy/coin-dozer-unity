using UnityEngine;

public class CoinCatcher : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CoinCatcherTrigger[] failTrigger;
    [SerializeField] private CoinCatcherTrigger[] winTrigger;

    private void Start()
    {
        SetupFailTrigger();
        SetupWinTrigger();
    }

    private void SetupWinTrigger()
    {
        foreach (var trigger in winTrigger)
        {
            trigger.Setup(this, true);
        }
    }

    private void SetupFailTrigger()
    {
        foreach (var trigger in failTrigger)
        {
            trigger.Setup(this, false);
        }
    }


    public void CallbackCoinCatcherTrigger(bool isWin, Coin coin)
    {
#if UNITY_EDITOR
        Debug.Log($"Coin fallen -> is win? = {isWin} value={coin.value}");
#endif
        if (isWin)
        {
            gameManager.AddScore(coin.value);
        }

        gameManager.CoinFallenDown(coin);
    }
}