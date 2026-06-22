using System;
using System.Collections;
using UnityEngine;

namespace JpPrototype4
{
    public class WaveManager : MonoBehaviour
    {
        [Tooltip("Wave configs in order. Each entry defines enemies and powerups for that wave.")]
        [SerializeField] private WaveConfig[] _waves;

        [Tooltip("Half-extents of the rectangular spawn area on X/Z.")]
        [SerializeField] private float _spawnRange = 9f;

        [Tooltip("Delay in seconds between waves.")]
        [SerializeField] private float _waveCooldown = 2f;

        // Raised when all waves are completed - wire up to GameManager/UI later.
        public event Action OnAllWavesCompleted;

        private int _currentWaveIndex;
        private int _activeEnemyCount;
        private bool _isWaveActive;
        private bool _allWavesCompleted;

        private void Start()
        {
            StartCoroutine(StartNextWave());
        }

        private void Update()
        {
            if (_allWavesCompleted || _isWaveActive || _activeEnemyCount > 0) return;

            if (_currentWaveIndex < _waves.Length)
            {
                StartCoroutine(StartNextWave());
            }
            else
            {
                _allWavesCompleted = true;
                OnAllWavesCompleted?.Invoke();
            }
        }

        private IEnumerator StartNextWave()
        {
            _isWaveActive = true;

            if (_currentWaveIndex > 0)
            {
                yield return new WaitForSeconds(_waveCooldown);
            }

            WaveConfig config = _waves[_currentWaveIndex];
            SpawnEnemies(config);
            SpawnPowerups(config);
            _currentWaveIndex++;

            _isWaveActive = false;
        }

        private void SpawnEnemies(WaveConfig config)
        {
            int successCount = 0;

            foreach (EnemyEntry entry in config.Enemies)
            {
                for (int i = 0; i < entry.Count; i++)
                {
                    GameObject instance = PoolManager.Instance.Get(entry.Prefab);
                    instance.transform.SetPositionAndRotation(GenerateSpawnPosition(), Quaternion.identity);

                    if (instance.TryGetComponent(out EnemyBall enemyBall))
                    {
                        enemyBall.OnReturnedToPool += HandleEnemyReturned;
                        successCount++;
                    }
                }
            }

            _activeEnemyCount = successCount;
        }

        private void SpawnPowerups(WaveConfig config)
        {
            foreach (PowerupEntry entry in config.Powerups)
            {
                for (int i = 0; i < entry.Count; i++)
                {
                    GameObject instance = PoolManager.Instance.Get(entry.Prefab);
                    instance.transform.SetPositionAndRotation(GenerateSpawnPosition(), Quaternion.identity);
                }
            }
        }

        private void HandleEnemyReturned(EnemyBall source)
        {
            source.OnReturnedToPool -= HandleEnemyReturned;
            _activeEnemyCount = Mathf.Max(0, _activeEnemyCount - 1);
        }

        private Vector3 GenerateSpawnPosition()
        {
            float x = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
            float z = UnityEngine.Random.Range(-_spawnRange, _spawnRange);
            return new Vector3(x, 0f, z);
        }
    }
}
