using UnityEngine;
using TMPro; // Thư viện bắt buộc cho TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField, Tooltip("Kéo object Car (chứa VehicleController) vào đây")]
    private VehicleController vehicle;

    [Header("UI Elements")]
    [SerializeField, Tooltip("Text hiển thị Tốc độ")]
    private TextMeshProUGUI speedText;

    [SerializeField, Tooltip("Text hiển thị Vòng tua máy")]
    private TextMeshProUGUI rpmText;

    [SerializeField, Tooltip("Text hiển thị Cấp số")]
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