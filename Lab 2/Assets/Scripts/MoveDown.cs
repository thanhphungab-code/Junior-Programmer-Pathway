using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float speed = 5;
    private float zDestroy = -10;
    private Rigidbody objectRb;
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        objectRb.AddForce(Vector3.forward * -speed);
        if (transform.position.z < zDestroy)
        {
            Destroy(gameObject);
        }
    }
}
