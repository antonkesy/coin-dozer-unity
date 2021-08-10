using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private UIManager uiManger;


    [SerializeField] private bool loadGameData;


    private void Start()
    {
        GameSaver.SaveData saveData = null;
        if (loadGameData)
        {
            saveData = GameSaver.LoadGame();
        }

        coinManager.StartCall(loadGameData, saveData);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    public void AddScore(int value)
    {
        levelManager.AddScore(value);
    }

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
}