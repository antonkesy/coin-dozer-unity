using antonkesy.MovableObjects;
using UnityEngine;

namespace antonkesy.PowerUps
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject wallPowerUpPrefab;
        [SerializeField] private WallsPowerUpManager wallPowerUpManager;

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
            }
        }

        internal void ActivateWallPowerUp()
        {
            wallPowerUpManager.Activate();
        }

        internal void SpawnWallPowerUpObject()
        {
            gameManager.AddPowerUp(wallPowerUpPrefab, new Vector3(0, 10F, 10F));
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