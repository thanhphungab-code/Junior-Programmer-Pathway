using UnityEngine;

namespace JpPrototype4
{
    public class ShieldAbility : IEquippable
    {
        public float Duration => 60f;
        public float Cooldown => 0f;

        private const float WeightMultiplier = 10f;
        private BaseBall _owner;

        public void OnEquip(BaseBall ball)
        {
            _owner = ball;
            _owner.Rb.mass *= WeightMultiplier;
            _owner.MoveSpeed *= WeightMultiplier;
        }

        public void OnUnequip()
        {
            if (_owner != null)
            {
                _owner.Rb.mass /= WeightMultiplier;
                _owner.MoveSpeed /= WeightMultiplier;
            }

            _owner = null;
        }

        public void OnUse() { }
        public void OnCollision(Collision collision) { }
    }
}