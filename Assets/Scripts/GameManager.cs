using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private UIManager uiManger;
    [SerializeField] private Shaker shaker;

    [SerializeField] private WallsPowerUpManager wallsPowerUpManager;

    [SerializeField] private bool loadGameData;


    private void Start()
    {
        GameSaver.SaveData saveData = null;

        if (loadGameData)
        {
            saveData = GameSaver.LoadGame();
        }

        coinManager.StartCall(loadGameData && saveData != null, saveData);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGameData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGameData();
        }
    }

    public void AddScore(int value)
    {
        levelManager.AddScore(value);
    }

    //TODO move to GameSaver
    private void SaveGameData()
    {
        var coins = coinManager.GetUsedCoins();

        var coinData = new List<GameSaver.SaveData.CoinData>(coins.Count);
        foreach (var coin in coins)
        {
            coinData.Add(new GameSaver.SaveData.CoinData(coin));
        }

        GameSaver.SaveGame(new GameSaver.SaveData(levelManager.levelScore, levelManager.level, coinData));
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

    public void CoinFallenDown(Coin coin)
    {
        coinManager.RemoveCoin(coin);
    }

    public void ActivateWallPowerUp()
    {
        wallsPowerUpManager.Activate();
    }
}