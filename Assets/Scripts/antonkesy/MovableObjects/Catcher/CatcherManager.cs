using System;
using UnityEngine;

namespace antonkesy.MovableObjects.Catcher
{
    public class CatcherManager : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private CatcherTrigger[] failTrigger;
        [SerializeField] private CatcherTrigger[] winTrigger;

        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
        }

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
            _gameManager.ProcessFallenPowerUp(powerUpObject, isWin);
        }

        internal void CallbackCoinTrigger(bool isWin, CoinObject coinObject)
        {
            coinObject.IsBeingDeleted = true;
            _gameManager.CoinFallenDown(coinObject, isWin);
        }
    }
}