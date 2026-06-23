using UnityEngine;

namespace JpPrototype4
{
    /// <summary>
    /// Passive ability that temporarily increases the push force multiplier of the ball.
    /// </summary>
    public class StrengthAbility : IEquippable
    {
        private float _powerUpStrength = 1000f;
        private BaseBall _owner;

        public float Duration => 8f;
        public float Cooldown => 0f;

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
        }

        public void OnCollision(Collision collision)
        {
            if (_owner == null) return;

            if (!collision.gameObject.TryGetComponent(out Rigidbody enemyRb)) return;

            Vector3 awayFromPlayer = collision.gameObject.transform.position - _owner.transform.position;
            enemyRb.AddForce(awayFromPlayer * _powerUpStrength, ForceMode.Force);
        }
    }
}