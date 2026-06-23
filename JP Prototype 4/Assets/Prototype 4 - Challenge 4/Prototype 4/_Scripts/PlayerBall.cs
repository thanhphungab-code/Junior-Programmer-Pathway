using UnityEngine;

namespace JpPrototype4
{
    public class PlayerBall : BaseBall
    {
        private EntityAbilityManager _abilityManager;

        [Tooltip("Reference to the InputManager on this GameObject.")]
        [SerializeField] private InputManager _inputManager;

        protected override void Awake()
        {
            base.Awake();
            _abilityManager = GetComponent<EntityAbilityManager>();
        }

        private void OnEnable()
        {
            if (_inputManager != null)
            {
                _inputManager.AbilityPressed += HandleAbilityInput;
            }
        }

        private void OnDisable()
        {
            if (_inputManager != null)
            {
                _inputManager.AbilityPressed -= HandleAbilityInput;
            }
        }

        private void FixedUpdate()
        {
            Vector2 input = _inputManager.MoveInput;

            Vector3 movement = new Vector3(input.x, 0.0f, input.y);
            Rb.AddForce(movement * MoveSpeed);
        }

        private void HandleAbilityInput()
        {
            if (_abilityManager != null)
            {
                _abilityManager.TryUseAbility();
            }
        }
    }
}