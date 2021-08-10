using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameSaver
{
    private static readonly string Path = $"{Application.persistentDataPath}/data.data";

    [Serializable]
    internal class SaveData
    {
        public long levelScore;
        public long level;
        public List<CoinData> coins;

        public SaveData(long levelScore, long level, List<CoinData> coins)
        {
            this.level = level;
            this.levelScore = levelScore;
            this.coins = coins;
        }

        public override string ToString()
        {
            return $"level={level}\tlevelScore={levelScore}\tcoinsCount={coins.Count}";
        }

        [Serializable]
        internal class CoinData
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 velocity;
            public Vector3 angularVelocity;
            public int value;

            internal CoinData(GameObject coin)
            {
                if (coin.CompareTag("Coin"))
                {
                    position = coin.transform.position;
                    rotation = coin.transform.rotation;
                    var rb = coin.GetComponent<Rigidbody>();
                    velocity = rb.velocity;
                    angularVelocity = rb.angularVelocity;
                    value = coin.GetComponent<Coin>().value;
                }
            }
        }
    }


    internal static void SaveGame(SaveData data)
    {
        var jsonString = JsonUtility.ToJson(data);
        try
        {
            var encrypted = Cryptor.EncryptStringToBytes(jsonString, Secrets.Key, Secrets.IV);
            File.WriteAllBytes(Path, encrypted);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    internal static SaveData LoadGame()
    {
        SaveData data = null;
        try
        {
            var readBytes = File.ReadAllBytes(Path);
            data = JsonUtility.FromJson<SaveData>(
                Cryptor.DecryptStringFromBytes(readBytes, Secrets.Key, Secrets.IV));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return data;
    }
}