using System.Collections.Generic;
using UnityEngine;

internal class AdvancedMovableObjectList
{
    [SerializeField] private PowerUpManager _powerUpManager;
    private readonly MovableObjectsManager _movableObjectsManager;
    private readonly LinkedList<int> _unusedObjectsIndex; //TODO check if too big and clean up
    internal List<MovableObject> MovableObjects { get; } //TODO own implementation of list with add returns index

    internal AdvancedMovableObjectList(MovableObjectsManager movableObjectsManager,
        PowerUpManager powerUpManager, int size = 50)
    {
        _powerUpManager = powerUpManager;
        _movableObjectsManager = movableObjectsManager;
        MovableObjects = new List<MovableObject>(size);
        _unusedObjectsIndex = new LinkedList<int>();

        BuildNewLinkedList(size);
        FillPoolWithCoins(size);
    }

    private void FillPoolWithCoins(int size)
    {
        for (var i = 0; i < size; ++i)
        {
            var newCoin = _movableObjectsManager.SpawnCoin(Vector3.zero);
            newCoin.gameObject.SetActive(false);
            newCoin.Index = i;
            MovableObjects.Add(newCoin);
        }
    }

    private void BuildNewLinkedList(int size)
    {
        for (var i = 0; i < size; ++i)
        {
            _unusedObjectsIndex.AddLast(i);
        }
    }

    internal void Remove(MovableObject movableObject)
    {
        movableObject.SetUnused();
        _unusedObjectsIndex.AddLast(movableObject.Index);
        if (movableObject.GetType() != typeof(CoinObject))
        {
            //convert spot to coin 
            //TODO find better solution
            //todo save overwrites objects
            MovableObjects[movableObject.Index] = AddNewObject(_movableObjectsManager.SpawnCoin(Vector3.zero));
        }
    }

    internal void AddCoinFromSaveData(GameSaver.SaveData.CoinSaveData coinSaveData)
    {
        var newMovableObject =
            AddCoin(coinSaveData.position);

        ((CoinObject)newMovableObject).value = coinSaveData.value;

        SetPositionFromSaveData(newMovableObject, coinSaveData);
    }

    internal void AddPowerUpFromSaveData(GameSaver.SaveData.PowerUpSaveData powerUpSaveData)
    {
        var newMovableObject =
            AddPowerUp(_powerUpManager.GetPowerUpPrefab(powerUpSaveData.type), powerUpSaveData.position);
        SetPositionFromSaveData(newMovableObject, powerUpSaveData);
    }

    internal MovableObject AddPowerUp(GameObject powerUpPrefab, Vector3 position)
    {
        return AddNewObject(_movableObjectsManager.SpawnPowerUp(powerUpPrefab, position));
    }

    internal MovableObject AddCoin(Vector3 position)
    {
        var unusedCoinSpot = GetUnusedCoinIndex();
        if (unusedCoinSpot >= 0)
        {
            var coin = MovableObjects[unusedCoinSpot];
            coin.transform.localPosition = position;
            coin.gameObject.SetActive(true);
            return coin;
        }

        //if no free coins available
        //!! ASSUMING add always to end of list
        //TODO fix or own implementation
        return AddNewObject(_movableObjectsManager.SpawnCoin(position));
    }

    private MovableObject AddNewObject(MovableObject newObject)
    {
        var count = MovableObjects.Count;
        newObject.Index = count;
        MovableObjects.Add(newObject);
        return newObject;
    }

    private int GetUnusedCoinIndex()
    {
        var index = -1;
        var firsUnusedCoin = _unusedObjectsIndex.First;
        if (firsUnusedCoin != null)
        {
            index = _unusedObjectsIndex.First.Value;
            _unusedObjectsIndex.RemoveFirst();
        }

        return index;
    }

    private void SetPositionFromSaveData(MovableObject newMovableObject,
        GameSaver.SaveData.MovableObjectSaveData movableObjectSaveData)
    {
        newMovableObject.gameObject.transform.rotation = movableObjectSaveData.rotation;
        newMovableObject.Rb.velocity = movableObjectSaveData.velocity;
        newMovableObject.Rb.angularVelocity = movableObjectSaveData.angularVelocity;
    }
}