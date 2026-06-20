using System.Collections;
using UnityEngine;

namespace JpPrototype4
{
    public class WaveManager : MonoBehaviour
    {
        [Tooltip("Enemy prefabs to randomly draw from each wave. Each must have EnemyBall and PooledObject components.")]
        [SerializeField] private GameObject[] _enemyPrefabs;

        [Tooltip("Powerup prefab spawned once per wave.")]
        [SerializeField] private GameObject _powerupPrefab;

        [Tooltip("Half-extents of the rectangular spawn area on X/Z.")]
        [SerializeField] private float _spawnRange = 9f;

        [Tooltip("Delay in seconds between waves.")]
        [SerializeField] private float _waveCooldown = 2f;

        private int _waveNumber = 1;
        private int _activeEnemyCount;
        private bool _isWaveActive;

        private void Start()
        {
            StartCoroutine(StartNextWave());
        }

        private void Update()
        {
            if (!_isWaveActive && _activeEnemyCount <= 0)
            {
                StartCoroutine(StartNextWave());
            }
        }

        private IEnumerator StartNextWave()
        {
            _isWaveActive = true;

            if (_waveNumber > 1)
            {
                yield return new WaitForSeconds(_waveCooldown);
            }

            SpawnWave(_waveNumber);
            SpawnPowerup();
            _waveNumber++;

            _isWaveActive = false;
        }

        private void SpawnWave(int count)
        {
            int successCount = 0;

            for (int i = 0; i < count; i++)
            {
                GameObject prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
                GameObject instance = PoolManager.Instance.Get(prefab);
                instance.transform.SetPositionAndRotation(GenerateSpawnPosition(), Quaternion.identity);

                if (instance.TryGetComponent(out EnemyBall enemyBall))
                {
                    enemyBall.OnReturnedToPool += HandleEnemyReturned;
                    successCount++;
                }
            }

            _activeEnemyCount = successCount;
        }

        private void SpawnPowerup()
        {
            if (_powerupPrefab == null) return;

            GameObject instance = PoolManager.Instance.Get(_powerupPrefab);
            instance.transform.SetPositionAndRotation(GenerateSpawnPosition(), Quaternion.identity);
        }

        private void HandleEnemyReturned(EnemyBall source)
        {
            source.OnReturnedToPool -= HandleEnemyReturned;
            _activeEnemyCount = Mathf.Max(0, _activeEnemyCount - 1);
        }

        private Vector3 GenerateSpawnPosition()
        {
            float x = Random.Range(-_spawnRange, _spawnRange);
            float z = Random.Range(-_spawnRange, _spawnRange);
            return new Vector3(x, 0f, z);
        }
    }
}
