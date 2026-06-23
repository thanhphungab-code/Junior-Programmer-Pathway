using UnityEngine;

namespace JpPrototype4
{
    [System.Serializable]
    public class EnemyEntry
    {
        [Tooltip("Enemy prefab to spawn. Must have EnemyBall and PooledObject components.")]
        public GameObject Prefab;

        [Tooltip("Number of this enemy type to spawn in this wave.")]
        public int Count;
    }

    [System.Serializable]
    public class PowerupEntry
    {
        [Tooltip("Powerup prefab to spawn. Must have PowerupPickup and PooledObject components.")]
        public GameObject Prefab;

        [Tooltip("Number of this powerup type to spawn in this wave.")]
        public int Count;
    }

    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Scriptable Objects/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [Tooltip("List of enemy types and their counts for this wave.")]
        public EnemyEntry[] Enemies;

        [Tooltip("List of powerup types and their counts for this wave.")]
        public PowerupEntry[] Powerups;
    }
}
