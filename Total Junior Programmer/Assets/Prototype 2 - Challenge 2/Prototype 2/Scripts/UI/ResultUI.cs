using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Prototype2
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI animalCountText;
        [SerializeField] private TextMeshProUGUI diamondCountText;
        [SerializeField] private TextMeshProUGUI coinCountText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button closeButton;

        private int score, animalCount, diamondCount, coinCount;

        private void Awake()
        {
            retryButton.onClick.AddListener(OnRetryButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        public void ShowResult(int score, int animalCount, int diamondCount, int coinCount)
        {
            this.score = score;
            this.animalCount = animalCount;
            this.diamondCount = diamondCount;
            this.coinCount = coinCount;

            scoreText.text = "0";
            animalCountText.text = "0";
            diamondCountText.text = "0";
            coinCountText.text = "0";
            gameObject.SetActive(true);
            PlayAnimation();
        }

        private void OnRetryButtonClicked()
        {
            GameManager.Instance.StartGame();
            gameObject.SetActive(false);
        }

        private void PlayAnimation()
        {
            float duration = 0.8f;
            float stagger = 0.15f;

            Tween.Custom(0f, (float)score, duration, x => scoreText.text = ((int)x).ToString(), Ease.OutCubic);
            Tween.Delay(stagger, () => Tween.Custom(0f, (float)animalCount, duration, x => animalCountText.text = ((int)x).ToString(), Ease.OutCubic));
            Tween.Delay(stagger * 2, () => Tween.Custom(0f, (float)diamondCount, duration, x => diamondCountText.text = ((int)x).ToString(), Ease.OutCubic));
            Tween.Delay(stagger * 3, () => Tween.Custom(0f, (float)coinCount, duration, x => coinCountText.text = ((int)x).ToString(), Ease.OutCubic));
        }

        private void OnCloseButtonClicked()
        {
            GameManager.Instance.GoHome();
            gameObject.SetActive(false);
        }
    }
}