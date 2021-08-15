using System.Collections.Generic;
using System.Linq;
using antonkesy.PowerUps;
using antonkesy.Utility;
using UnityEngine;

namespace antonkesy.MovableObjects
{
    public class MovableObjectsManager : MonoBehaviour
    {
        [SerializeField] private Transform leftWall;
        private float _leftWallStartX;
        [SerializeField] private Transform rightWall;
        private float _rightWallStartX;
        [SerializeField] private Transform backWall;
        private float _backWallStartZ;
        [SerializeField] private int objectsInPool = 50;
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private float coinSpawnHeight;
        private PowerUpManager _powerUpManager;
        private AdvancedMovableObjectList _movableObjectList;
        private MovableObjectRemoveManager _movableObjectRemoveManager;


        private void Awake()
        {
            _powerUpManager = GetComponent<PowerUpManager>();
            _movableObjectRemoveManager = GetComponent<MovableObjectRemoveManager>();
            _movableObjectList = new AdvancedMovableObjectList(this, _powerUpManager, objectsInPool);
            _movableObjectRemoveManager.SetUp(_movableObjectList);
            CalculateWallStart();
        }

        private void CalculateWallStart()
        {
            _leftWallStartX = leftWall.transform.position.x - leftWall.transform.localScale.x / 2;
            _rightWallStartX = rightWall.transform.position.x + rightWall.transform.localScale.x / 2;
            _backWallStartZ = backWall.transform.position.z + backWall.transform.localScale.z / 2;
        }


        internal void StartCall(bool loadSaveData, GameSaver.SaveData saveData)
        {
            if (loadSaveData)
            {
                SetCoinsFromSavedPosition(saveData);
            }
            else
            {
                SetCoinsNewStartPos();
            }
        }

        private void SetCoinsFromSavedPosition(GameSaver.SaveData savedData)
        {
            foreach (var coin in savedData.coins)
            {
                _movableObjectList.AddCoinFromSaveData(coin);
            }

            foreach (var powerUp in savedData.powerUps)
            {
                _movableObjectList.AddPowerUpFromSaveData(powerUp);
            }
        }

        internal void AddCoinClickPos(Vector3 hitInfoPoint)
        {
            hitInfoPoint.y = coinSpawnHeight;
            hitInfoPoint = AdjustCoinInWallPosition(hitInfoPoint);
            _movableObjectList.AddCoin(hitInfoPoint);
        }

        private Vector3 AdjustCoinInWallPosition(Vector3 coinPos)
        {
            var coinRadius = coinPrefab.transform.localScale.x / 2;

            if (coinPos.x + coinRadius >= _leftWallStartX)
            {
                coinPos.x = _leftWallStartX - coinRadius;
            }

            if (coinPos.x - coinRadius <= _rightWallStartX)
            {
                coinPos.x = _rightWallStartX + coinRadius;
            }

            if (coinPos.z - coinRadius <= _backWallStartZ)
            {
                coinPos.z = _backWallStartZ + coinRadius;
            }

            return coinPos;
        }


        private void SetCoinsNewStartPos()
        {
            const int newStartRows = 6;
            const int newStartColumns = 6;

            const int smallRowCount = 4;
            const float spacer = .1f;
            var coinRadius = coinPrefab.transform.localScale.x;
            var zPosCoin = 3.5F; //z start pos
            var xPosCoin = coinRadius * smallRowCount / 2 - coinRadius / 2 + spacer * smallRowCount / 2;
            var spacePerCoin = coinRadius + spacer;
            //small row
            SetRowCoins(xPosCoin, zPosCoin, spacePerCoin, smallRowCount);
            //full rows
            for (var rows = 0; rows < newStartRows; ++rows)
            {
                xPosCoin = coinRadius * newStartColumns / 2 - coinRadius / 2 + spacer * newStartColumns / 2;
                zPosCoin += coinRadius + spacer;
                SetRowCoins(xPosCoin, zPosCoin, spacePerCoin, newStartColumns);
            }
        }

        private void SetRowCoins(float xPosStart, float zPos, float spacePerCoin, int amount)
        {
            for (var column = 0; column < amount; ++column)
            {
                var coin = _movableObjectList.AddCoin(new Vector3(xPosStart, 0, zPos));
                coin.gameObject.SetActive(true);
                xPosStart -= spacePerCoin;
            }
        }

        internal List<MovableObject> GetUsedMovableObjects()
        {
            return _movableObjectList.MovableObjects.Where(obj => obj.gameObject.activeSelf && !obj.IsBeingDeleted)
                .ToList();
        }

        internal MovableObject SpawnCoin(Vector3 position)
        {
            var newCoin = Instantiate(coinPrefab, transform);
            newCoin.transform.localPosition = position;
            var movAbleObjectScript = newCoin.GetComponent<MovableObject>();
            return movAbleObjectScript;
        }

        internal void RemoveMovableObject(MovableObject movableObject)
        {
            _movableObjectRemoveManager.RemoveCoin(movableObject);
        }

        internal MovableObject SpawnPowerUp(GameObject powerUpPrefab, Vector3 position)
        {
            var newPowerUp = Instantiate(powerUpPrefab, transform);
            newPowerUp.transform.localPosition = position;
            return newPowerUp.GetComponent<MovableObject>();
        }

        internal void AddPowerUp(GameObject powerUpPrefab, Vector3 position)
        {
            _movableObjectList.AddPowerUp(powerUpPrefab, position);
        }
    }
}