using System.Collections;
using UnityEngine;

namespace JpPrototype4
{
    /// <summary>
    /// Manages particle system pooling and playback.
    /// Provides APIs for persistent and one-shot particle effects.
    /// </summary>
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Play a particle effect that persists and follows the anchor.
        /// </summary>
        /// <param name="prefab">ParticleSystem prefab to play</param>
        /// <param name="anchor">Transform to parent the particle to</param>
        /// <returns>Handle (GameObject) to stop later. Store for cleanup in OnDisable.</returns>
        public GameObject PlayPersistent(ParticleSystem prefab, Transform anchor)
        {
            if (prefab == null || anchor == null)
                return null;

            GameObject instance = PoolManager.Instance.Get(prefab.gameObject);
            if (instance == null)
                return null;

            instance.transform.SetParent(anchor, worldPositionStays: false);
            instance.transform.localPosition = Vector3.zero;

            ParticleSystem ps = instance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            return instance;
        }

        /// <summary>
        /// Play a one-shot particle effect at a specific position and rotation.
        /// Automatically returns to pool when particle finishes.
        /// </summary>
        /// <param name="prefab">ParticleSystem prefab to play</param>
        /// <param name="position">World position to spawn at</param>
        /// <param name="rotation">World rotation to spawn with</param>
        public void PlayOnce(ParticleSystem prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
                return;

            GameObject instance = PoolManager.Instance.Get(prefab.gameObject);
            if (instance == null)
                return;

            instance.transform.SetPositionAndRotation(position, rotation);

            ParticleSystem ps = instance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                StartCoroutine(WaitForParticleAndReturn(ps, instance));
            }
        }

        /// <summary>
        /// Stop a persistent particle effect and return it to the pool.
        /// </summary>
        /// <param name="handle">GameObject handle returned from PlayPersistent</param>
        public void Stop(GameObject handle)
        {
            if (handle == null)
                return;

            ParticleSystem ps = handle.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop();
            }

            handle.SetActive(false);

            PooledObject pooledObj = handle.GetComponent<PooledObject>();
            if (pooledObj != null)
            {
                pooledObj.ReturnToPool();
            }
            else
            {
                Destroy(handle);
            }
        }

        private IEnumerator WaitForParticleAndReturn(ParticleSystem particleSystem, GameObject instance)
        {
            yield return new WaitUntil(() => !particleSystem.IsAlive());

            instance.SetActive(false);

            PooledObject pooledObj = instance.GetComponent<PooledObject>();
            if (pooledObj != null)
            {
                pooledObj.ReturnToPool();
            }
            else
            {
                Destroy(instance);
            }
        }
    }
}
