using UnityEngine;

namespace JpPrototype4
{
    public class DashAbility : IEquippable
    {
        public float Duration => 10f;
        public float Cooldown => 2f;

        private float _dashForce = 10f;
        private BaseBall _owner;

        public void OnEquip(BaseBall ball)
        {
            _owner = ball;
        }

        public void OnUse()
        {
            if (_owner == null) return;

            Vector3 dashDirection = Vector3.zero;

            if (_owner is PlayerBall)
            {
                dashDirection = _owner.Rb.linearVelocity.normalized;
                if (dashDirection == Vector3.zero)
                {
                    dashDirection = _owner.transform.forward;
                }
            }
            else if (_owner is EnemyBall)
            {
                PlayerBall player = Object.FindFirstObjectByType<PlayerBall>();

                if (player != null)
                {
                    dashDirection = (player.transform.position - _owner.transform.position).normalized;
                }
                else
                {
                    dashDirection = _owner.transform.forward;
                }
            }

            dashDirection.y = 0;
            _owner.Rb.AddForce(dashDirection.normalized * _dashForce, ForceMode.Impulse);
        }

        public void OnUnequip()
        {
            _owner = null;
        }

        public void OnCollision(Collision collision) { }
    }
}