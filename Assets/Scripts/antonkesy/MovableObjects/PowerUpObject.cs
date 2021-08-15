using UnityEngine;

namespace antonkesy.MovableObjects
{
    public class PowerUpObject : MovableObject
    {
        internal enum PowerUpType
        {
            Wall
        }

        [SerializeField] internal PowerUpType type;
    }
}