using UnityEngine;

namespace JpPrototype4
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseBall : MonoBehaviour
    {
        [Header("Base Settings")]
        [Tooltip("Base movement force applied to the ball each physics step.")]
        [SerializeField] private float _moveSpeed = 15f;

        [Tooltip("Projectile prefab used if this ball has the Shoot ability.")]
        [SerializeField] private GameObject _projectilePrefab;
        public GameObject ProjectilePrefab { get => _projectilePrefab; protected set => _projectilePrefab = value; }
        [Tooltip("Target layer for the Shoot ability to aim at.")]
        [SerializeField] private LayerMask _targetLayer;
        public LayerMask TargetLayer { get => _targetLayer; protected set => _targetLayer = value; }
        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

        public Rigidbody Rb { get; private set; }
        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Finds the position of the nearest object on the given layer within a search radius.
        /// </summary>
        /// <param name="targetLayer">The layer mask to search on.</param>
        /// <param name="searchRadius">The radius to search within.</param>
        /// <returns>The world position of the nearest target, or <see cref="Vector3.zero"/> if none found.</returns>
        public Vector3 GetNearestEnemyPosition(LayerMask targetLayer, float searchRadius = 20f)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, targetLayer);
            float closestDistance = Mathf.Infinity;
            Vector3 closestPos = Vector3.zero;
            foreach (Collider hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPos = hit.transform.position;
                }
            }
            return closestPos;
        }
    }
}