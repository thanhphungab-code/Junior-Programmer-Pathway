using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JpPrototype4
{
    public class InputManager : MonoBehaviour
    {
        private InputSystem_Actions _gameInput;

        public Vector2 MoveInput { get; private set; }

        public event Action AbilityPressed;

        private void Awake()
        {
            _gameInput = new InputSystem_Actions();

            _gameInput.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _gameInput.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

            _gameInput.Player.UseAbility.performed += ctx => AbilityPressed?.Invoke();
        }

        private void OnEnable()
        {
            _gameInput.Player.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Player.Disable();
        }
    }
}