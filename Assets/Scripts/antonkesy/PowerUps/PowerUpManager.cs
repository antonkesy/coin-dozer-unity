using System;
using antonkesy.MovableObjects;
using UnityEngine;

namespace antonkesy.PowerUps
{
    public class PowerUpManager : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private GameObject wallPowerUpPrefab;
        [SerializeField] private WallsPowerUpManager wallPowerUpManager;

        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
        }

        internal void ProcessPowerUpDrop(PowerUpObject powerUpGameObject)
        {
#if UNITY_EDITOR
            Debug.Log("Power Up falling");
#endif
            switch (powerUpGameObject.type)
            {
                case PowerUpObject.PowerUpType.Wall:
                    ActivateWallPowerUp();
                    break;
                case PowerUpObject.PowerUpType.Shake:
                    _gameManager.Shake();
                    break;
            }
        }

        internal void ActivateWallPowerUp()
        {
            wallPowerUpManager.Activate();
        }

        internal void SpawnWallPowerUpObject()
        {
            _gameManager.AddPowerUp(wallPowerUpPrefab, new Vector3(0, 10F, 10F));
        }

        internal GameObject GetPowerUpPrefab(PowerUpObject.PowerUpType type)
        {
            var powerUp = type switch
            {
                PowerUpObject.PowerUpType.Wall => wallPowerUpPrefab,
                _ => null
            };

            return powerUp;
        }
    }
}