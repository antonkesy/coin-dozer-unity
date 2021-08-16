using UnityEngine;

namespace antonkesy.MovableObjects
{
    public class PowerUpObject : MovableObject
    {
        internal enum PowerUpType
        {
            Wall,
            Shake
        }

        [SerializeField] internal PowerUpType type;
    }
}