using UnityEngine;

namespace antonkesy.MovableObjects.Catcher
{
    public class CatcherTrigger : MonoBehaviour
    {
        private bool _isWin;
        private CatcherManager _catcherManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                _catcherManager.CallbackCoinTrigger(_isWin, other.GetComponent<CoinObject>());
            }
            else if (other.CompareTag("PowerUp"))
            {
                _catcherManager.CallbackPowerUpTrigger(_isWin, other.GetComponent<PowerUpObject>());
            }
        }


        public void Setup(CatcherManager catcherManager, bool isWin)
        {
            _catcherManager = catcherManager;
            _isWin = isWin;
        }
    }
}