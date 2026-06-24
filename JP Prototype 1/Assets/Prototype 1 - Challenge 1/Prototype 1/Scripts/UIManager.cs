using UnityEngine;
using TMPro; // Thư viện bắt buộc cho TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField, Tooltip("Reference to the VehicleController component on the Car object.")]
    private VehicleController vehicle;

    [Header("UI Elements")]
    [SerializeField, Tooltip("TextMeshPro component to display the current speed.")]
    private TextMeshProUGUI speedText;

    [SerializeField, Tooltip("TextMeshPro component to display the current engine RPM.")]
    private TextMeshProUGUI rpmText;

    [SerializeField, Tooltip("TextMeshPro component to display the current gear.")]
    private TextMeshProUGUI gearText;

    void Update()
    {
        if (vehicle == null || speedText == null || rpmText == null || gearText == null) return;

        speedText.text = $"{Mathf.RoundToInt(vehicle.CurrentSpeed)} km/h";
        rpmText.text = $"RPM: {Mathf.RoundToInt(vehicle.CurrentRPM)}";

        int currentGear = vehicle.CurrentGear;
        gearText.text = $"GEAR: {currentGear}";
    }
}