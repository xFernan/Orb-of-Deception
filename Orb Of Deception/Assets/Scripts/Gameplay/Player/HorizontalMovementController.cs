using System;
using OrbOfDeception.Core.Input;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class HorizontalMovementController
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly float _groundVelocity;
        private readonly float _airVelocity;
        private readonly InputManager _inputManager;

        private float MoveDirection => _inputManager.GetHorizontal();
        public static Action<int> onDirectionChanged;
        
        public float Direction { get; private set; }
        public bool IsMoving { get; private set; }

        public HorizontalMovementController(Rigidbody2D rigidbody, float groundVelocity, float airVelocity, InputManager inputManager)
        {
            _rigidbody = rigidbody;
            _groundVelocity = groundVelocity;
            _airVelocity = airVelocity;
            _inputManager = inputManager;
        }
        
        public void Update()
        {
            var moveDirection = MoveDirection;
            
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
            var direction = MoveDirection;
            
            var velocity = _rigidbody.velocity;
            var velocityX = GameManager.Player.GroundDetector.IsOnTheGround() ? _groundVelocity : _airVelocity;
            velocity.x = velocityX * direction;
            _rigidbody.velocity = velocity;
        }
    }
}