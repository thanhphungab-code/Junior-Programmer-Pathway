using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace JpPrototype4
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        private readonly Dictionary<int, ObjectPool<GameObject>> _pools = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        /// <summary>
        /// Retrieves an active instance from the pool for the given prefab.
        /// Creates a new pool automatically if one does not yet exist.
        /// </summary>
        public GameObject Get(GameObject prefab)
        {
            ObjectPool<GameObject> pool = GetOrCreatePool(prefab);
            GameObject instance = pool.Get();

            if (instance.TryGetComponent(out PooledObject pooledObject))
            {
                pooledObject.SourcePrefab = prefab;
            }

            return instance;
        }

        /// <summary>
        /// Returns an instance back to its originating pool.
        /// </summary>
        public void Release(GameObject prefab, GameObject instance)
        {
            if (!_pools.TryGetValue(prefab.GetInstanceID(), out ObjectPool<GameObject> pool))
            {
                Object.Destroy(instance);
                return;
            }

            pool.Release(instance);
        }

        private ObjectPool<GameObject> GetOrCreatePool(GameObject prefab)
        {
            int key = prefab.GetInstanceID();

            if (_pools.TryGetValue(key, out ObjectPool<GameObject> existing))
            {
                return existing;
            }

            ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
                createFunc: () => Object.Instantiate(prefab),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                actionOnDestroy: obj => Object.Destroy(obj)
            );

            _pools.Add(key, newPool);
            return newPool;
        }
    }
}
