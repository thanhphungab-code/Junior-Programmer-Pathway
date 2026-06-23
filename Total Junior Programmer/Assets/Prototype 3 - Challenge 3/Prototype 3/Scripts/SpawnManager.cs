using UnityEngine;
namespace Prototype_3
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject obstaclePrefab;
        private Vector3 spawnPos = new Vector3(25, 0, 0);
        private float startDelay = 3;
        private float repeatRate = 3;
        private PlayerController playerController;
        void Start()
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        }

        public void SpawnObstacle()
        {
            if (playerController.gameOver) return;
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }
    }
}