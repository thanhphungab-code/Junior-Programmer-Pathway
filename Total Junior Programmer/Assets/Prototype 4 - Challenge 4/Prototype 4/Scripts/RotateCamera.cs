using JpPrototype4;
using UnityEngine;
namespace Prototype4
{
    public class RotateCamera : MonoBehaviour
    {
        public float rotationSpeed;
        private InputSystem_Actions controls;

        private void Awake()
        {
            controls = new InputSystem_Actions();
        }
        private void OnEnable()
        {
            controls.Player.Enable();
        }

        private void Update()
        {
            Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
            float horizontalInput = moveInput.x;
            transform.Rotate(-Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }
    }
}
