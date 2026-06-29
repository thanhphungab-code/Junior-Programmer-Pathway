using UnityEngine;
using UnityEngine.UI;
namespace Prototype2
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            GameManager.Instance.GoHome();
            gameObject.SetActive(false);
        }

        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }

    }
}