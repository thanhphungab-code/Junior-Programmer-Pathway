using UnityEngine;
using UnityEngine.UI;
namespace Prototype2
{
    public class HomeUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingButton;
        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            settingButton.onClick.AddListener(OnSettingButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            GameManager.Instance.StartGame();
            gameObject.SetActive(false);
        }

        private void OnSettingButtonClicked()
        {
            GameManager.Instance.SettingUI.gameObject.SetActive(true);
        }
    }
}