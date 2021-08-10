using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private UIManager uiManger;
    private int _levelScore;
    private int _level;
    private int _xpPerLevel = 10;

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
        _levelScore += value;
        //TODO check level/save & more
        uiManger.UpdateScore(_levelScore);
    }

    private void SaveGameData()
    {
        var coins = coinManager.GetUsedCoins();

        var coinData = new List<GameSaver.SaveData.CoinData>(coins.Count);
        foreach (var coin in coins)
        {
            coinData.Add(new GameSaver.SaveData.CoinData(coin));
        }

        GameSaver.SaveGame(new GameSaver.SaveData(_levelScore, _level, coinData));
    }
}