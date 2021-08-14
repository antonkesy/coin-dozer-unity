using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private MovableObjectsManager movableObjectsManager;
    [SerializeField] private UIManager uiManger;
    [SerializeField] private Shaker shaker;
    [SerializeField] private PowerUpManager powerUpManager;

    [SerializeField] private bool loadGameData;


    private void Start()
    {
        GameSaver.SaveData saveData = null;

        if (loadGameData)
        {
            saveData = GameSaver.LoadGame();
        }

        movableObjectsManager.StartCall(loadGameData && saveData != null, saveData);
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
        levelManager.AddScore(value);
    }

    //TODO move to GameSaver
    private void SaveGameData()
    {
        var usedMovableObjects = movableObjectsManager.GetUsedMovableObjects();

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

        GameSaver.SaveGame(new GameSaver.SaveData(levelManager.levelScore, levelManager.level, coinData, powerUpData));
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
        shaker.StartShaking(3);
    }

    public void CoinFallenDown(CoinObject coinObject, bool isWin)
    {
        if (isWin)
        {
            AddScore(coinObject.value);
        }

        movableObjectsManager.RemoveMovableObject(coinObject);
    }

    internal void ProcessFallenPowerUp(PowerUpObject powerUpObject, bool isWin)
    {
        powerUpManager.ProcessPowerUpDrop(powerUpObject);
        movableObjectsManager.RemoveMovableObject(powerUpObject);
    }

    public void ActivateWallPowerUp()
    {
        powerUpManager.ActivateWallPowerUp();
    }

    public void AddPowerUp(GameObject wallPowerUpManager, Vector3 vector3)
    {
        movableObjectsManager.AddPowerUp(wallPowerUpManager, vector3);
    }
}