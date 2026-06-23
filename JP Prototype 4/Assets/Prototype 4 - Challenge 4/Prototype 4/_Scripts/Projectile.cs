using System.Collections;
using UnityEngine;

namespace JpPrototype4
{
    [RequireComponent(typeof(PooledObject))]
    public class Projectile : MonoBehaviour
    {
        [Tooltip("Seconds before the projectile auto-returns to pool.")]
        [SerializeField] private float _lifetime = 3f;

        [Tooltip("Layers the projectile is destroyed upon hitting.")]
        [SerializeField] private LayerMask _hitLayers;

        private PooledObject _pooledObject;
        private Coroutine _lifetimeCoroutine;
        private bool _isReturning;

        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
        }

        private void OnEnable()
        {
            _isReturning = false;
            _lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime());
        }

        private void OnDisable()
        {
            if (_lifetimeCoroutine != null)
            {
                StopCoroutine(_lifetimeCoroutine);
                _lifetimeCoroutine = null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_hitLayers.value == 0 || (_hitLayers.value & (1 << collision.gameObject.layer)) != 0)
            {
                SafeReturn();
            }
        }

        private IEnumerator ReturnAfterLifetime()
        {
            yield return new WaitForSeconds(_lifetime);
            SafeReturn();
        }

        private void SafeReturn()
        {
            if (_isReturning) return;

            _isReturning = true;
            _pooledObject.ReturnToPool();
        }
    }
}
