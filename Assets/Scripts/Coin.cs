using UnityEngine;

public class Coin : MonoBehaviour
{
    internal int Index { set; get; }
    internal Rigidbody Rb { private set; get; }
    [SerializeField] public int value = 1;

    private void Start()
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
    }
}