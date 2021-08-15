using System;
using System.Collections.Generic;
using antonkesy.MovableObjects;
using antonkesy.MovableObjects.Shaker;
using antonkesy.PowerUps;
using antonkesy.Utility;
using UnityEngine;

namespace antonkesy
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManger;
        [SerializeField] private ShakerController shakerController;
        private LevelManager _levelManager;
        private MovableObjectsManager _movableObjectsManager;
        private PowerUpManager _powerUpManager;

        [SerializeField] private bool loadGameData;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
            _movableObjectsManager = GetComponent<MovableObjectsManager>();
            _powerUpManager = GetComponent<PowerUpManager>();
        }

        private void Start()
        {
            GameSaver.SaveData saveData = null;

            if (loadGameData)
            {
                saveData = GameSaver.LoadGame();
            }

            _movableObjectsManager.StartCall(loadGameData && saveData != null, saveData);
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveGameData();
            }
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveGameData();
            }
        }

        private void AddScore(int value)
        {
            _levelManager.AddScore(value);
        }

        //TODO move to GameSaver
        private void SaveGameData()
        {
            var usedMovableObjects = _movableObjectsManager.GetUsedMovableObjects();

            var coinData = new List<GameSaver.SaveData.CoinSaveData>(usedMovableObjects.Count);
            var powerUpData = new List<GameSaver.SaveData.PowerUpSaveData>(usedMovableObjects.Count);

            foreach (var movableObject in usedMovableObjects)
            {
                if (movableObject.GetType() == typeof(CoinObject))
                {
                    coinData.Add(new GameSaver.SaveData.CoinSaveData(movableObject));
                }
                else if (movableObject.GetType() == typeof(PowerUpObject))
                {
                    powerUpData.Add(new GameSaver.SaveData.PowerUpSaveData(movableObject));
                }
            }

            GameSaver.SaveGame(new GameSaver.SaveData(_levelManager.levelScore, _levelManager.level, coinData,
                powerUpData));
        }

        public void UpdateScoreUI(long levelScore, long scorePerLevel)
        {
            uiManger.UpdateScore(levelScore, scorePerLevel);
        }

        public void UpdateLevelUI(long level)
        {
            uiManger.UpdateLevel(level);
        }

        public void Shake()
        {
            shakerController.StartShaking(3);
        }

        public void CoinFallenDown(CoinObject coinObject, bool isWin)
        {
            if (isWin)
            {
                AddScore(coinObject.value);
            }

            _movableObjectsManager.RemoveMovableObject(coinObject);
        }

        internal void ProcessFallenPowerUp(PowerUpObject powerUpObject, bool isWin)
        {
            _powerUpManager.ProcessPowerUpDrop(powerUpObject);
            _movableObjectsManager.RemoveMovableObject(powerUpObject);
        }

        public void ActivateWallPowerUp()
        {
            _powerUpManager.ActivateWallPowerUp();
        }

        public void AddPowerUp(GameObject wallPowerUpManager, Vector3 vector3)
        {
            _movableObjectsManager.AddPowerUp(wallPowerUpManager, vector3);
        }
    }
}