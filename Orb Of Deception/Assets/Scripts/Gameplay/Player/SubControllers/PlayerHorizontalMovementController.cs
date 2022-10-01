using System;
using OrbOfDeception.Core.Input;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerHorizontalMovementController
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly float _groundVelocity;
        private readonly float _airVelocity;
        private readonly InputManager _inputManager;

        private float MoveDirection => GameManager.Player.isControlled ? _inputManager.GetHorizontal() : 0;
        
        public int Orientation { get; private set; }
        public bool IsMoving { get; private set; }

        public PlayerHorizontalMovementController(Rigidbody2D rigidbody, float groundVelocity, float airVelocity, InputManager inputManager)
        {
            _rigidbody = rigidbody;
            _groundVelocity = groundVelocity;
            _airVelocity = airVelocity;
            _inputManager = inputManager;
        }
        
        public void Update()
        {
            var moveDirection = MoveDirection;
            
            if (!IsMoving && moveDirection != 0 && GameManager.Player.GroundDetector.IsOnTheGround())
                GameManager.Player.soundsPlayer.Play("StartMove");
            
            IsMoving = moveDirection != 0;
            
            if (IsMoving)
            {
                Orientation = (int) moveDirection;
            }
        }
        
        public void FixedUpdate()
        {
            var direction = MoveDirection;
            
            var velocity = _rigidbody.velocity;
            var velocityX = GameManager.Player.GroundDetector.IsOnTheGround() ? _groundVelocity : _airVelocity;
            velocity.x = velocityX * direction;

            /*var groundNormalVector = GameManager.Player.GroundDetector.GetGroundNormalVector();
            if (groundNormalVector != Vector3.zero)
                velocity = Vector3.ProjectOnPlane(velocity, groundNormalVector);*/
            
            _rigidbody.velocity = velocity;
        }

        public void SetDirection(int newDirection)
        {
            Orientation = newDirection;
        }
    }
}