using Prototype2;
using UnityEngine;
using UnityEngine.UI;
namespace Prototype2
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private Button settingButton;

        private void Awake()
        {
            settingButton.onClick.AddListener(OnSettingButtonClicked);
        }

        private void OnSettingButtonClicked()
        {
            GameManager.Instance.SettingUI.gameObject.SetActive(true);
        }
    }
}
