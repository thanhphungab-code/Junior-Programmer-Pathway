using UnityEngine;

namespace JpPrototype4
{
    /// <summary>
    /// Defines the contract for any equippable ability.
    /// </summary>
    public interface IEquippable
    {
        float Duration { get; }
        float Cooldown { get; }

        void OnEquip(BaseBall ball);
        void OnUnequip();
        void OnUse();

        // Kept so abilities like Dash can apply push logic on collision.
        void OnCollision(Collision collision);
    }
}
