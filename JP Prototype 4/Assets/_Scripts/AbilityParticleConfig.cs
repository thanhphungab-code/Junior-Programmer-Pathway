using UnityEngine;

namespace JpPrototype4
{
    /// <summary>
    /// Configuration for a single ability's particle effects.
    /// Stores references to persistent indicator and activation particles,
    /// plus tuning parameters for positioning.
    /// </summary>
    [CreateAssetMenu(fileName = "AbilityParticleConfig", menuName = "Abilities/AbilityParticleConfig")]
    public class AbilityParticleConfig : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Particle system that plays while ability is active (indicator)")]
        private ParticleSystem _activeIndicator;

        [SerializeField]
        [Tooltip("Particle system that plays when ability is activated (effect)")]
        private ParticleSystem _onActivation;

        [SerializeField]
        [Tooltip("Position offset for activation particle (ability-specific meaning)")]
        private Vector3 _activationOffset = Vector3.zero;

        public ParticleSystem ActiveIndicator => _activeIndicator;
        public ParticleSystem OnActivation => _onActivation;
        public Vector3 ActivationOffset => _activationOffset;
    }
}
