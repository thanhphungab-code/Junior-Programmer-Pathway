using UnityEngine;

namespace JpPrototype4
{
    /// <summary>
    /// Active ability that fires a projectile towards the nearest enemy.
    /// </summary>
    public class ShootAbility : IEquippable
    {
        private GameObject _projectilePrefab;
        private LayerMask _targetLayer;
        private float _shootForce = 50f;
        private BaseBall _owner;

        public float Duration => 10f;
        public float Cooldown => 0.2f;

        public ShootAbility(GameObject prefab, LayerMask layer)
        {
            _projectilePrefab = prefab;
            _targetLayer = layer;
        }

        public void OnEquip(BaseBall ball)
        {
            _owner = ball;
        }

        public void OnUnequip()
        {
            _owner = null;
        }

        public void OnUse()
        {
            if (_owner == null || _projectilePrefab == null) return;

            Vector3 targetPos = _owner.GetNearestEnemyPosition(_targetLayer);

            Vector3 shootDirection = (targetPos == Vector3.zero)
                ? _owner.Rb.linearVelocity.normalized
                : (targetPos - _owner.transform.position).normalized;

            shootDirection.y = 0;

            if (shootDirection == Vector3.zero)
            {
                shootDirection = _owner.transform.forward;
            }

            Vector3 spawnPos = _owner.transform.position + shootDirection * 1.5f;

            GameObject projectile = PoolManager.Instance.Get(_projectilePrefab);
            projectile.transform.SetPositionAndRotation(spawnPos, Quaternion.LookRotation(shootDirection));

            if (projectile.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(shootDirection * _shootForce, ForceMode.Impulse);
            }
        }

        public void OnCollision(Collision collision)
        {
        }
    }
}