using System.Collections.Generic;
using UnityEngine;

namespace antonkesy.MovableObjects
{
    public class MovableObjectRemoveManager : MonoBehaviour
    {
        private class CoinDeleteData
        {
            public readonly MovableObject MovableObject;
            public readonly float DeletionTime;

            internal CoinDeleteData(MovableObject movableObject)
            {
                MovableObject = movableObject;
                DeletionTime = Time.time + 10F;
            }
        }

        private readonly Queue<CoinDeleteData> _removeCoinQue = new Queue<CoinDeleteData>();

        private AdvancedMovableObjectList _advancedMovableObjectList;


        internal void SetUp(AdvancedMovableObjectList advancedMovableObjectList)
        {
            _advancedMovableObjectList = advancedMovableObjectList;
        }


        private void Update()
        {
            CheckRemoveCoinQue();
        }


        public void RemoveCoin(MovableObject movableObject)
        {
            _removeCoinQue.Enqueue(new CoinDeleteData(movableObject));
        }

        private void CheckRemoveCoinQue()
        {
            if (_removeCoinQue.Count > 0 && _removeCoinQue.Peek().DeletionTime < Time.time)
            {
                _advancedMovableObjectList.Remove(_removeCoinQue.Dequeue().MovableObject);
            }
        }
    }
}