using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public InputAction moveAction;
    private Rigidbody playerRb;
    private Vector2 moveInput;
    public float zBound;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        moveAction.Enable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        ConstrainPlayerPosition();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        playerRb.AddForce(moveDirection * speed);
    }
    public void ConstrainPlayerPosition()
    {
        if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
    }
}
