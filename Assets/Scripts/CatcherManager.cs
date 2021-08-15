using UnityEngine;

public class CatcherManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CatcherTrigger[] failTrigger;
    [SerializeField] private CatcherTrigger[] winTrigger;

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


    internal void CallbackPowerUpTrigger(bool isWin, PowerUpObject powerUpObject)
    {
        powerUpObject.IsBeingDeleted = true;
        gameManager.ProcessFallenPowerUp(powerUpObject, isWin);
    }

    internal void CallbackCoinTrigger(bool isWin, CoinObject coinObject)
    {
        coinObject.IsBeingDeleted = true;
        gameManager.CoinFallenDown(coinObject, isWin);
    }
}