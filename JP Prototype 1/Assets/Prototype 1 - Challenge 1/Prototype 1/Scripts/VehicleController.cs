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
        // Tính toán RPM dựa trên vận tốc và giới hạn tốc độ của số hiện tại
        currentRPM = maxRPM / gearMaxSpeeds[currentGear - 1] * currentSpeed;

        // Logic sang số tự động (Auto-Transmission)
        if (currentRPM > 7500f && currentGear < 4)
        {
            currentGear++;
            // Cập nhật lại RPM ngay lập tức để đồng bộ với cấp số mới
            currentRPM = maxRPM / gearMaxSpeeds[currentGear - 1] * currentSpeed;
        }
        else if (currentRPM < 3000f && currentGear > 1)
        {
            // Tránh việc về số khi xe đang ở số 1
            currentGear--;
            currentRPM = maxRPM / gearMaxSpeeds[currentGear - 1] * currentSpeed;
        }
    }

    private void ApplyMotorForce()
    {
        // Chỉ đẩy xe khi có nhấn ga và tốc độ chưa vượt quá giới hạn thiết kế (180 km/h)
        if (Mathf.Abs(gasInput) > 0.05f && currentSpeed < gearMaxSpeeds[3])
        {
            // Tính gia tốc gốc (m/s^2) dựa trên bảng thiết kế
            float targetSpeedMS = gearMaxSpeeds[currentGear - 1] / 3.6f;
            float requiredAcceleration = targetSpeedMS / gearAccelTimes[currentGear - 1];

            // Hệ số nhân (Tweak multiplier) để bù đắp các thất thoát vật lý khác của môi trường 3D.
            // Nếu thấy xe vẫn chậm, bạn hãy tăng số 2.5f này lên 4f hoặc 5f.
            float finalAcceleration = requiredAcceleration * 2.5f;

            Vector3 forceVector = transform.forward * gasInput * finalAcceleration;

            // Dùng ForceMode.Acceleration để ép gia tốc, bỏ qua tác động của khối lượng (Mass)
            rb.AddForce(forceVector, ForceMode.Acceleration);
        }
    }
    // --- LOGIC GIAI ĐOẠN 4: XỬ LÝ LỰC CẢN VÀ PHANH ---
    private void HandleDeceleration()
    {
        // Chỉ áp dụng lực cản nếu xe đang chạy (tránh làm xe tự giật lùi khi đang đứng yên)
        if (currentSpeed > 0.1f)
        {
            float currentDeceleration = 0f;

            // Ưu tiên 1: Nếu người chơi đang nhấn phanh (Space)
            if (brakeInput > 0.5f)
            {
                currentDeceleration = brakeForce;
            }
            // Ưu tiên 2: Nếu không nhấn phanh VÀ nhả ga hoàn toàn -> Áp dụng phanh động cơ
            else if (Mathf.Abs(gasInput) < 0.05f)
            {
                currentDeceleration = idleDeceleration;
            }

            // Thực thi lực cản
            if (currentDeceleration > 0f)
            {
                // rb.linearVelocity.normalized lấy ra hướng di chuyển (bỏ qua độ lớn)
                // Thêm dấu âm (-) để lực đẩy ngược lại hướng di chuyển
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
        // 1. Vận tốc đi thẳng: Chiếu vector vận tốc hiện tại lên trục dọc của xe (transform.forward)
        Vector3 forwardVelocity = transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward);

        // 2. Vận tốc trượt ngang: Chiếu vector vận tốc hiện tại lên trục ngang của xe (transform.right)
        Vector3 rightVelocity = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);

        // 3. Tổng hợp lại vận tốc mới: Giữ nguyên đà đi thẳng, nhưng triệt tiêu đà trượt ngang dựa trên tireGrip
        // Nếu tireGrip = 0.95, lực trượt ngang chỉ còn giữ lại 5% (1 - 0.95)
        rb.linearVelocity = forwardVelocity + rightVelocity * (1f - tireGrip);
    }
}
