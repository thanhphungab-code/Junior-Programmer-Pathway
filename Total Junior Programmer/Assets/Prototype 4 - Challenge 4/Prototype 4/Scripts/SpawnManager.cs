using UnityEngine;
namespace Prototype4
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public GameObject powerUpPrefab;
        public float spawnRange = 9;
        public int enemyCount;
        public int waveNumber = 1;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SpawnEnemyWave(waveNumber);
            SpawnPowerUp();
        }

        // Update is called once per frame
        void Update()
        {
            enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
            if (enemyCount == 0)
            {
                waveNumber++;
                SpawnEnemyWave(waveNumber);
                SpawnPowerUp();
            }
        }

        private void SpawnEnemyWave(int enemiesToSpawn)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            }
        }

        private void SpawnPowerUp()
        {
            Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
        }

        private Vector3 GenerateSpawnPosition()
        {
            float spawnPosX = Random.Range(-spawnRange, spawnRange);
            float spawnPosZ = Random.Range(-spawnRange, spawnRange);
            return new Vector3(spawnPosX, 0, spawnPosZ);
        }
    }
}