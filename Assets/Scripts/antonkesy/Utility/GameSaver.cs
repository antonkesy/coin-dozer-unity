using System;
using System.Collections.Generic;
using System.IO;
using antonkesy.MovableObjects;
using UnityEngine;

namespace antonkesy.Utility
{
    public static class GameSaver
    {
        private static readonly string Path = $"{Application.persistentDataPath}/data.data";

        private static bool _dontSave;

        [Serializable]
        internal class SaveData
        {
            public long levelScore;
            public long level;
            public List<CoinSaveData> coins;
            public List<PowerUpSaveData> powerUps;

            public SaveData(long levelScore, long level, List<CoinSaveData> coins, List<PowerUpSaveData> powerUps)
            {
                this.level = level;
                this.levelScore = levelScore;
                this.powerUps = powerUps;
                this.coins = coins;
            }

            public override string ToString()
            {
                return $"level={level}\tlevelScore={levelScore}\tcoinsCount={powerUps.Count}";
            }

            [Serializable]
            internal class MovableObjectSaveData
            {
                public Vector3 position;
                public Quaternion rotation;
                public Vector3 velocity;
                public Vector3 angularVelocity;

                //Todo fix for saving objects
                internal MovableObjectSaveData(MovableObject movableObject)
                {
                    position = movableObject.transform.position;
                    rotation = movableObject.transform.rotation;
                    var rb = movableObject.GetComponent<Rigidbody>();
                    velocity = rb.velocity;
                    angularVelocity = rb.angularVelocity;
                }
            }

            [Serializable]
            internal class CoinSaveData : MovableObjectSaveData
            {
                public int value;

                internal CoinSaveData(MovableObject movableObject) : base(movableObject)
                {
                    value = movableObject.GetComponent<CoinObject>().value;
                }
            }

            [Serializable]
            internal class PowerUpSaveData : MovableObjectSaveData
            {
                public PowerUpObject.PowerUpType type;

                internal PowerUpSaveData(MovableObject movableObject) : base(movableObject)
                {
                    type = movableObject.GetComponent<PowerUpObject>().type;
                }
            }
        }


        internal static void SaveGame(SaveData data)
        {
            if (_dontSave) return;

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

        public static void ResetData()
        {
            _dontSave = true;
            File.WriteAllBytes(Path, new byte[] { });
        }
    }
}