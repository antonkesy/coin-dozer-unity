using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCatcher : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CoinCatcherTrigger[] failTrigger;
    [SerializeField] private CoinCatcherTrigger[] winTrigger;

    void Start()
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


    public void CallbackCoinCatcherTrigger(bool isWin, int value)
    {
#if UNITY_EDITOR
        Debug.Log($"Coin fallen -> is win? = {isWin} value={value}");
#endif
        if (isWin)
        {
            gameManager.AddScore(value);
        }
    }
}