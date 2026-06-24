using UnityEngine;

public class WheelVisualizer : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField, Tooltip("Reference to the VehicleController for speed and steering data.")]
    private VehicleController vehicle;

    [Header("Wheel Transforms")]
    [SerializeField, Tooltip("Front Left Wheel Mesh")]
    private Transform frontLeftWheel;

    [SerializeField, Tooltip("Front Right Wheel Mesh")]
    private Transform frontRightWheel;

    [SerializeField, Tooltip("Rear Left Wheel Mesh")]
    private Transform rearLeftWheel;

    [SerializeField, Tooltip("Rear Right Wheel Mesh")]
    private Transform rearRightWheel;

    [Header("Wheel Settings")]
    [SerializeField, Tooltip("Radius of the wheels in meters. Used to calculate accurate rolling speed.")]
    private float wheelRadius = 0.35f;

    private float currentWheelRotation = 0f;

    void Update()
    {
        if (vehicle == null) return;

        float speedMS = vehicle.CurrentSpeed / 3.6f;

        float circumference = 2f * Mathf.PI * wheelRadius;

        float rotationThisFrame = (speedMS * Time.deltaTime / circumference) * 360f;
        currentWheelRotation += rotationThisFrame * vehicle.MoveDirection;

        if (frontLeftWheel != null)
            frontLeftWheel.localRotation = Quaternion.Euler(currentWheelRotation, vehicle.CurrentSteerAngle, 0f);

        if (frontRightWheel != null)
            frontRightWheel.localRotation = Quaternion.Euler(currentWheelRotation, vehicle.CurrentSteerAngle, 0f);

        if (rearLeftWheel != null)
            rearLeftWheel.localRotation = Quaternion.Euler(currentWheelRotation, 0f, 0f);

        if (rearRightWheel != null)
            rearRightWheel.localRotation = Quaternion.Euler(currentWheelRotation, 0f, 0f);
    }
}