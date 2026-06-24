using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField, Tooltip("The main vehicle transform to follow.")]
    private Transform carTarget;

    [SerializeField, Tooltip("The transform representing the first-person camera position inside the vehicle.")]
    private Transform fpsPoint;

    [Header("Third Person Settings")]
    [SerializeField, Tooltip("Distance behind the target in third-person view.")]
    private float distance = 7f;

    [SerializeField, Tooltip("Height above the target in third-person view.")]
    private float height = 5.5f;

    [SerializeField, Tooltip("Smoothing factor for camera movement and rotation.")]
    private float smoothSpeed = 10f;

    [Header("Orbit Settings")]
    [SerializeField, Tooltip("Sensitivity of mouse orbit rotation.")]
    private float mouseSensitivity = 0.2f;

    [SerializeField, Tooltip("Time in seconds before the camera auto-aligns behind the car after no mouse input.")]
    private float autoAlignDelay = 2f;

    [SerializeField, Tooltip("Speed at which the camera auto-aligns behind the car.")]
    private float autoAlignSpeed = 3f;

    [Header("Input (New Input System)")]
    [SerializeField, Tooltip("Action to toggle between first and third person view.")]
    private InputAction toggleViewAction;

    private float mouseX = 0f;
    private float mouseY = 15f;
    private float lastMouseTime;
    private bool isFirstPerson = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        toggleViewAction.Enable();
    }

    private void OnDisable()
    {
        toggleViewAction.Disable();
    }

    void Update()
    {
        if (toggleViewAction.WasPressedThisFrame())
        {
            isFirstPerson = !isFirstPerson;

            mouseX = 0f;
            mouseY = isFirstPerson ? 0f : 15f;
        }
    }

    void LateUpdate()
    {
        if (carTarget == null) return;

        HandleMouseInput();

        Quaternion currentRotation = Quaternion.Euler(mouseY, carTarget.eulerAngles.y + mouseX, 0);

        UpdateCameraTransform(currentRotation);
    }

    private void HandleMouseInput()
    {
        Vector2 mouseDelta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;

        if (mouseDelta.sqrMagnitude > 0.1f)
        {
            mouseX += mouseDelta.x * mouseSensitivity;
            mouseY -= mouseDelta.y * mouseSensitivity;
            mouseY = Mathf.Clamp(mouseY, -20f, 60f);
            lastMouseTime = Time.time;
        }
        else if (!isFirstPerson && Time.time - lastMouseTime > autoAlignDelay)
        {
            mouseX = Mathf.Lerp(mouseX, 0f, autoAlignSpeed * Time.deltaTime);
            mouseY = Mathf.Lerp(mouseY, 15f, autoAlignSpeed * Time.deltaTime);
        }
    }

    private void UpdateCameraTransform(Quaternion currentRotation)
    {
        if (isFirstPerson && fpsPoint != null)
        {
            transform.position = fpsPoint.position;
            transform.rotation = currentRotation;
        }
        else
        {
            Vector3 targetPosition = carTarget.position - (currentRotation * Vector3.forward * distance) + (Vector3.up * height);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, smoothSpeed * Time.deltaTime);
        }
    }
}