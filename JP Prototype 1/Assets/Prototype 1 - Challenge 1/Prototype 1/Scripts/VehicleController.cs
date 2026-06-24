using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    public float currentSpeed;
    [SerializeField] private float maxSteerAngleAtZero = 45f;
    [SerializeField] private float maxSteerAngleAtMaxSpeed = 10f;
    [SerializeField] private float steerTime = 1.5f;
    [SerializeField] private float turnSensitivity = 2.5f;
    [SerializeField] private float currentSteerAngle;
    [SerializeField] private int currentGear = 1;
    [SerializeField] private float currentRPM;
    [SerializeField] private float maxRPM = 8000f;

    [SerializeField] private float[] gearMaxSpeeds = { 50f, 90f, 140f, 180f };
    [SerializeField] private float[] gearAccelTimes = { 3f, 4f, 8f, 15f };
    [SerializeField] private float idleDeceleration = 5f;
    [SerializeField] private float brakeForce = 20f;
    [Header("Input (New Input System)")]
    [SerializeField] private InputAction driveAction;
    [SerializeField] private InputAction steerAction;
    [SerializeField] private InputAction brakeAction;
    [Range(0f, 1f)][SerializeField] private float tireGrip = 0.95f;
    private float gasInput;
    private float steerInput;
    private float brakeInput;
    private Rigidbody rb;

    public float CurrentSpeed => currentSpeed;
    public float CurrentRPM => currentRPM;
    public int CurrentGear => currentGear;
    public float CurrentSteerAngle => currentSteerAngle;
    public float MoveDirection => Vector3.Dot(rb.linearVelocity, transform.forward) >= 0 ? 1f : -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void OnEnable()
    {
        driveAction.Enable();
        steerAction.Enable();
        brakeAction.Enable();
    }

    private void OnDisable()
    {
        driveAction.Disable();
        steerAction.Disable();
        brakeAction.Disable();
    }

    void Update()
    {
        gasInput = driveAction.ReadValue<float>();
        steerInput = steerAction.ReadValue<float>();
        brakeInput = brakeAction.ReadValue<float>();
    }

    void FixedUpdate()
    {
        currentSpeed = rb.linearVelocity.magnitude * 3.6f;

        HandleEngine();
        ApplyMotorForce();
        HandleDeceleration();
        HandleSteering();

        HandleGrip();
    }
    private void HandleEngine()
    {
        currentRPM = maxRPM / gearMaxSpeeds[currentGear - 1] * currentSpeed;

        if (currentRPM > 7500f && currentGear < 4)
        {
            currentGear++;
            currentRPM = maxRPM / gearMaxSpeeds[currentGear - 1] * currentSpeed;
        }
        else if (currentRPM < 3000f && currentGear > 1)
        {
            currentGear--;
            currentRPM = maxRPM / gearMaxSpeeds[currentGear - 1] * currentSpeed;
        }
    }

    private void ApplyMotorForce()
    {
        if (Mathf.Abs(gasInput) > 0.05f && currentSpeed < gearMaxSpeeds[3])
        {
            float targetSpeedMS = gearMaxSpeeds[currentGear - 1] / 3.6f;
            float requiredAcceleration = targetSpeedMS / gearAccelTimes[currentGear - 1];
            float finalAcceleration = requiredAcceleration * 2.5f;
            Vector3 forceVector = transform.forward * gasInput * finalAcceleration;
            rb.AddForce(forceVector, ForceMode.Acceleration);
        }
    }
    private void HandleDeceleration()
    {
        if (currentSpeed < 0.5f && Mathf.Abs(gasInput) < 0.05f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return;
        }
        if (currentSpeed > 0.1f)
        {
            float currentDeceleration = 0f;

            if (brakeInput > 0.5f)
            {
                currentDeceleration = brakeForce;
            }
            else if (Mathf.Abs(gasInput) < 0.05f)
            {
                currentDeceleration = idleDeceleration;
            }

            if (currentDeceleration > 0f)
            {
                Vector3 decelerationForce = -rb.linearVelocity.normalized * currentDeceleration;
                rb.AddForce(decelerationForce, ForceMode.Acceleration);
            }
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

    private void HandleGrip()
    {
        Vector3 forwardVelocity = transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward);
        Vector3 rightVelocity = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + rightVelocity * (1f - tireGrip);
    }
}
