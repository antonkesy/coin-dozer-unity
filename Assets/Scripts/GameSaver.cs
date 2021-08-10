using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameSaver
{
    private static readonly string Path = $"{Application.persistentDataPath}/data";

    [Serializable]
    internal class SaveData
    {
        public long levelScore;
        public long level;
        public List<GameObject> coins;

        public SaveData(long levelScore, long level, List<GameObject> coins)
        {
            this.level = level;
            this.levelScore = levelScore;
            this.coins = coins;
        }

        public override string ToString()
        {
            return $"level={level}\tlevelScore={levelScore}\tcoinsCount={coins.Count}";
        }
    }


    internal static void SaveGame(SaveData data)
    {
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(Path, json);
    }

    internal static SaveData LoadGame()
    {
        var json = File.ReadAllText(Path);
        var savedData = JsonUtility.FromJson<SaveData>(json);
        return savedData;
    }
}