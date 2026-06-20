using UnityEngine;

namespace JpPrototype4
{
    [RequireComponent(typeof(Collider))]
    public class PowerupPickup : MonoBehaviour
    {
        [Header("Item Settings")]
        [Tooltip("The ability type granted to the player on pickup.")]
        [SerializeField] private AbilityType _abilityType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerBall player))
            {
                EntityAbilityManager manager = player.GetComponent<EntityAbilityManager>();
                if (manager != null)
                {
                    manager.EquipAbility(CreateAbility(player.ProjectilePrefab, player.TargetLayer));

                    if (TryGetComponent(out PooledObject pooledObject))
                    {
                        pooledObject.ReturnToPool();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        private IEquippable CreateAbility(GameObject projectilePrefab, LayerMask targetLayer)
        {
            switch (_abilityType)
            {
                case AbilityType.Dash:
                    return new DashAbility();
                case AbilityType.Shield:
                    return new ShieldAbility();
                case AbilityType.Shoot:
                    return new ShootAbility(projectilePrefab, targetLayer);
                case AbilityType.Slam:
                    return new SlamAbility();
                case AbilityType.Strength:
                    return new StrengthAbility();
                default:
                    return null;
            }
        }
    }
}