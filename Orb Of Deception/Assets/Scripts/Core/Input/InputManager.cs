using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OrbOfDeception.Core.Input
{
    public class InputManager : MonoBehaviour
    {
        private float _horizontal;
        private float _vertical;

        public Action Jump { set; private get; }
        public Action StopJumping { set; private get; }
        public Action<Vector2> Click { set; private get; }
        public Action<Vector2> DirectionalAttack { set; private get; }
        public Action ChangeOrbColor { set; private get; }

        public void OnMovement(InputAction.CallbackContext context)
        {
            var vectorInput = context.ReadValue<Vector2>();
            _horizontal = GetNormalizedValue(vectorInput.x);
            _vertical = GetNormalizedValue(vectorInput.y);
        }

        private int GetNormalizedValue(float decimalValue)
        {
            if (decimalValue == 0)
                return 0;
            if (decimalValue > 0)
                return 1;
            
            return -1;
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
                Jump?.Invoke();
            else if (context.canceled)
                StopJumping?.Invoke();
        }

        public float GetHorizontal()
        {
            return _horizontal;
        }

        public float GetVertical()
        {
            return _vertical;
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (!context.started)
                return;
            
            var mousePosition = Mouse.current.position.ReadValue();
            Click?.Invoke(mousePosition);
        }

        public void OnDirectionalAttack(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>().normalized;
            DirectionalAttack?.Invoke(direction);
        }

        public void OnChangeOrbColor(InputAction.CallbackContext context)
        {
            if (!context.started)
                return;
            
            ChangeOrbColor?.Invoke();
        }

        public void OnDirectionAttackButton(InputAction.CallbackContext context)
        {
            var direction = Gamepad.current.leftStick.ReadValue().normalized;
            DirectionalAttack?.Invoke(direction);
        }
    }
}