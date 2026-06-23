using UnityEngine;

namespace JpPrototype4
{
    /// <summary>
    /// Attach to every pooled prefab. Stores its source prefab reference
    /// and provides a single entry point for returning to the pool.
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        public GameObject SourcePrefab { get; set; }

        /// <summary>
        /// Returns this object to its originating pool.
        /// Falls back to Destroy if not managed by a pool.
        /// </summary>
        public void ReturnToPool()
        {
            if (SourcePrefab == null || PoolManager.Instance == null)
            {
                Destroy(gameObject);
                return;
            }

            PoolManager.Instance.Release(SourcePrefab, gameObject);
        }
    }
}
