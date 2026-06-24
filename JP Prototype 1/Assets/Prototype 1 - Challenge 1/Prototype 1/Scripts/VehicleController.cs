using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    [Header("Thông số Test Giai đoạn 1")]
    [SerializeField] private float baseForce = 15000f;
    public float currentSpeed;

    [Header("Đánh lái (Giai đoạn 2)")]
    [SerializeField] private float maxSteerAngleAtZero = 45f;
    [SerializeField] private float maxSteerAngleAtMaxSpeed = 10f;
    [SerializeField] private float steerTime = 1.5f;
    [SerializeField] private float turnSensitivity = 2.5f;
    [SerializeField] private float currentSteerAngle;

    [Header("Input (New Input System)")]
    [SerializeField] private InputAction driveAction;
    [SerializeField] private InputAction steerAction;

    private float gasInput;
    private float steerInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void OnEnable()
    {
        driveAction.Enable();
        steerAction.Enable();
    }

    private void OnDisable()
    {
        driveAction.Disable();
        steerAction.Disable();
    }

    void Update()
    {
        gasInput = driveAction.ReadValue<float>();
        steerInput = steerAction.ReadValue<float>();
    }

    void FixedUpdate()
    {
        currentSpeed = rb.linearVelocity.magnitude * 3.6f;
        ApplyMotorForce();
        HandleSteering();
    }

    private void ApplyMotorForce()
    {
        if (Mathf.Abs(gasInput) > 0.05f)
        {
            Vector3 pushForce = transform.forward * gasInput * baseForce;
            rb.AddForce(pushForce, ForceMode.Force);
        }
    }

    private void HandleSteering()
    {
        float speedRatio = Mathf.Clamp01(currentSpeed / 180f);
        float currentMaxAngle = Mathf.Lerp(maxSteerAngleAtZero, maxSteerAngleAtMaxSpeed, speedRatio);

        float targetAngle = steerInput * currentMaxAngle;
        float steerSpeed = currentMaxAngle / steerTime;

        currentSteerAngle = Mathf.MoveTowards(currentSteerAngle, targetAngle, steerSpeed * Time.fixedDeltaTime);
        if (currentSpeed > 1f)
        {
            float direction = Vector3.Dot(rb.linearVelocity, transform.forward) > 0 ? 1 : -1;
            float turnAmount = currentSteerAngle * turnSensitivity * Time.fixedDeltaTime * direction;
            Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}
