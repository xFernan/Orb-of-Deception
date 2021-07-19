using System;
using OrbOfDeception.Core.Input;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class HorizontalMovementController
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly float _velocity;
        private readonly InputManager _inputManager;
        
        public static Action<int> onDirectionChanged;
        
        public float Direction { get; private set; }
        public bool IsMoving { get; private set; }

        public HorizontalMovementController(Rigidbody2D rigidbody, float velocity, InputManager inputManager)
        {
            _rigidbody = rigidbody;
            _velocity = velocity;
            _inputManager = inputManager;
        }
        
        public void Update()
        {
            var moveDirection = _inputManager.GetHorizontal();

            IsMoving = moveDirection != 0;
            
            if (IsMoving)
            {
                var haveDirectionChanged = Direction != moveDirection;

                Direction = moveDirection;
                if (haveDirectionChanged)
                    onDirectionChanged((int) moveDirection);
            }
        }
        
        public void FixedUpdate()
        {
            var newVelocity = _rigidbody.velocity;
            newVelocity.x = _velocity * _inputManager.GetHorizontal();
            _rigidbody.velocity = newVelocity;
        }
    }
}