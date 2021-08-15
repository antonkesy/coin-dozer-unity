using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovableObjectsManager : MonoBehaviour
{
    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private int objectsInPool = 50;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnHeight;

    private AdvancedMovableObjectList _movableObjectList;

    [SerializeField] private MovableObjectRemoveManager movableObjectRemoveManager;

    private void Awake()
    {
        _movableObjectList = new AdvancedMovableObjectList(this, powerUpManager, objectsInPool);
        movableObjectRemoveManager.SetUp(_movableObjectList);
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
        _movableObjectList.AddCoin(hitInfoPoint);
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
        movableObjectRemoveManager.RemoveCoin(movableObject);
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