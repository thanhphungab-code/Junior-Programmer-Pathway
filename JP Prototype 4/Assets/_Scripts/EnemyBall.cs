using System;
using UnityEngine;

namespace JpPrototype4
{
    public class EnemyBall : BaseBall
    {
        [Tooltip("Interval in seconds between each ability assignment cycle.")]
        [SerializeField] private float _timeToGetAbility = 5f;

        [Header("Fixed Ability Setup")]
        [Tooltip("The ability type assigned to this enemy.")]
        [SerializeField] private AbilityType _fixedAbility;

        [Tooltip("Y position below which the enemy is returned to pool.")]
        [SerializeField] private float _outOfBoundsY = -10f;

        public event Action<EnemyBall> OnReturnedToPool;

        private EntityAbilityManager _abilityManager;
        private PooledObject _pooledObject;
        private Transform _playerTarget;
        private float _abilityTimer;

        protected override void Awake()
        {
            base.Awake();
            _abilityManager = GetComponent<EntityAbilityManager>();
            _pooledObject = GetComponent<PooledObject>();

            PlayerBall player = FindFirstObjectByType<PlayerBall>();
            if (player != null)
            {
                _playerTarget = player.transform;
            }
        }

        private void OnEnable()
        {
            _abilityTimer = 0f;

            if (_playerTarget == null)
            {
                PlayerBall player = FindFirstObjectByType<PlayerBall>();
                if (player != null)
                {
                    _playerTarget = player.transform;
                }
            }
        }

        private void Update()
        {
            _abilityTimer += Time.deltaTime;
            if (_abilityTimer >= _timeToGetAbility)
            {
                AssignFixedAbility();
                _abilityTimer = 0f;
            }

            if (_abilityManager != null)
            {
                _abilityManager.TryUseAbility();
            }

            if (transform.position.y < _outOfBoundsY)
            {
                ReturnToPool();
            }
        }

        private void FixedUpdate()
        {
            if (_playerTarget == null) return;

            Vector3 direction = (_playerTarget.position - transform.position).normalized;
            direction.y = 0;

            Rb.AddForce(direction * MoveSpeed);
        }

        private void AssignFixedAbility()
        {
            IEquippable newAbility = null;

            switch (_fixedAbility)
            {
                case AbilityType.Dash:
                    newAbility = new DashAbility();
                    break;
                case AbilityType.Shoot:
                    if (ProjectilePrefab != null)
                    {
                        newAbility = new ShootAbility(ProjectilePrefab, TargetLayer);
                    }
                    break;
                case AbilityType.Slam:
                    newAbility = new SlamAbility();
                    break;
                case AbilityType.Shield:
                    newAbility = new ShieldAbility();
                    break;
                case AbilityType.Strength:
                    newAbility = new StrengthAbility();
                    break;
                default:
                    break;
            }

            if (newAbility != null)
            {
                _abilityManager.EquipAbility(newAbility);
            }
        }

        private void ReturnToPool()
        {
            OnReturnedToPool?.Invoke(this);

            if (_pooledObject != null)
            {
                _pooledObject.ReturnToPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}