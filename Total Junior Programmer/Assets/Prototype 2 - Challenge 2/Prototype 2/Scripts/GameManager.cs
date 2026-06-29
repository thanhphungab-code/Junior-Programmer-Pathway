using UnityEngine;
using Prototype2;
namespace Prototype2
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private PlayerController playerPrefab;

        [Header("UI References")]
        [SerializeField] private HomeUI homeUI;
        [SerializeField] private GameplayUI gameplayUI;
        [SerializeField] private ResultUI resultUI;
        [SerializeField] private SettingUI settingUI;

        public HomeUI HomeUI => homeUI;
        public GameplayUI GameplayUI => gameplayUI;
        public ResultUI ResultUI => resultUI;
        public SettingUI SettingUI => settingUI;

        private PlayerController player;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            homeUI.gameObject.SetActive(true);
            gameplayUI.gameObject.SetActive(false);
            resultUI.gameObject.SetActive(false);
            settingUI.gameObject.SetActive(false);
        }

        public void StartGame()
        {
            spawnManager.StartSpawning();
            player = Instantiate(playerPrefab, Vector3.zero, playerPrefab.transform.rotation);
            gameplayUI.gameObject.SetActive(true);
        }

        public void GameOver()
        {
            spawnManager.StopSpawning();
            spawnManager.RemoveAllAnimals();
            if (player != null)
            {
                Destroy(player.gameObject);
            }
            gameplayUI.gameObject.SetActive(false);
            resultUI.ShowResult(100000, 80, 30, 1000); // Example values for score, animalCount, diamondCount, coinCount
        }

        public void GoHome()
        {
            spawnManager.StopSpawning();
            if (player != null)
            {
                Destroy(player.gameObject);
            }
            homeUI.gameObject.SetActive(true);
        }

    }
}