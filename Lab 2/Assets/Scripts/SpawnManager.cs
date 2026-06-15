using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject powerUp;

    private float zEnemySpawn = 12;
    private float xSpawnRange = 16;
    private float zPowerUpRange = 5;
    private float ySpawn = 0.75f;
    public float powerUpSpawnTime = 5;
    public float enemySpawnTime = 1;
    public float startDelay = 1;

    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", startDelay, enemySpawnTime);
        InvokeRepeating("SpawnPowerUp", startDelay, powerUpSpawnTime);
    }

    public void SpawnRandomEnemy()
    {
        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        int randomIndex = Random.Range(0, enemies.Length);
        Vector3 spawnPos = new Vector3(randomX, ySpawn, zEnemySpawn);
        Instantiate(enemies[randomIndex], spawnPos, enemies[randomIndex].gameObject.transform.rotation);
    }

    public void SpawnPowerUp()
    {
        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        float randomZ = Random.Range(-zPowerUpRange, zPowerUpRange);
        Vector3 spawnPos = new Vector3(randomX, ySpawn, randomZ);
        Instantiate(powerUp, spawnPos, powerUp.gameObject.transform.rotation);
    }

}
