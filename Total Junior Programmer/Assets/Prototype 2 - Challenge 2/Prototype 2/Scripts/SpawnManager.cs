using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype2
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject[] animalPrefabs;
        public float spawnRangeX = 20;
        public float spawnPosZ = 20;
        private float startDelay = 2;
        private float spawnInterval = 1.5f;

        public void StartSpawning()
        {
            InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
        }

        public void StopSpawning()
        {
            CancelInvoke("SpawnRandomAnimal");
        }

        public void SpawnRandomAnimal()
        {
            int animalIndex = Random.Range(0, animalPrefabs.Length);
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
            Instantiate(animalPrefabs[animalIndex], spawnPos, animalPrefabs[0].transform.rotation, transform);
        }

        public void RemoveAllAnimals()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

    }
}