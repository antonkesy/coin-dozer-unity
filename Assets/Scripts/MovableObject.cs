using UnityEngine;

public class MovableObject : MonoBehaviour
{
    internal bool IsBeingDeleted;
    internal int Index { set; get; }
    internal Rigidbody Rb { private set; get; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }


    public void SetUnused()
    {
        Rb.angularVelocity = Vector3.zero;
        Rb.velocity = Vector3.zero;
        Rb.rotation = Quaternion.Euler(Vector3.zero);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        gameObject.SetActive(false);
        IsBeingDeleted = false;
    }
}