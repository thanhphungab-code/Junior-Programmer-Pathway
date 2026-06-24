using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    [Header("Thông số Test Giai đoạn 1")]
    public float baseForce = 15000f;
    public float currentSpeed;

    [Header("Input (New Input System)")]
    // Khai báo InputAction trực tiếp để gán trên Inspector
    public InputAction driveAction;

    private float gasInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    // Bắt buộc phải Enable/Disable InputAction để nó hoạt động
    private void OnEnable()
    {
        driveAction.Enable();
    }

    private void OnDisable()
    {
        driveAction.Disable();
    }

    void Update()
    {
        // Đọc giá trị float từ trục dọc (trả về từ -1 đến 1)
        gasInput = driveAction.ReadValue<float>();
    }

    void FixedUpdate()
    {
        currentSpeed = rb.linearVelocity.magnitude * 3.6f;

        Debug.Log("Tốc độ: " + Mathf.RoundToInt(currentSpeed) + " km/h | Input: " + gasInput);

        ApplyMotorForce();
    }

    private void ApplyMotorForce()
    {
        if (Mathf.Abs(gasInput) > 0.05f)
        {
            Vector3 pushForce = transform.forward * gasInput * baseForce;
            rb.AddForce(pushForce, ForceMode.Force);
        }
    }
}
