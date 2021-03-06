using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class JumpController
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly GroundDetector _groundDetector;
        private readonly float _jumpForce;
        private readonly float _jumpTime;
        private readonly float _maxFallVelocity;
        
        private float _jumpTimeCounter;
        private bool _isJumping;

        public JumpController(Rigidbody2D rigidbody, float jumpForce, float jumpTime, float maxFallVelocity, GroundDetector groundDetector)
        {
            _rigidbody = rigidbody;
            _groundDetector = groundDetector;
            _jumpForce = jumpForce;
            _jumpTime = jumpTime;
            _maxFallVelocity = maxFallVelocity;
        }

        public void Jump()
        {
            if (!_isJumping && (!_groundDetector.IsOnTheGround() && !_groundDetector.IsOnCoyoteTime()))
                return;
            
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _isJumping = true;
            _jumpTimeCounter = 0;
        }

        public void StopJumping()
        {
            _isJumping = false;
        }
        
        public void FixedUpdate()
        {
            var newVelocity = _rigidbody.velocity;
            
            if (_isJumping) {
                newVelocity.y = _jumpForce;
            
                _jumpTimeCounter += Time.deltaTime;
                if (_jumpTimeCounter >= _jumpTime)
                {
                    _isJumping = false;
                }
            }
            else
            {
                newVelocity.y = Mathf.Max(_maxFallVelocity, newVelocity.y);
            }
            
            _rigidbody.velocity = newVelocity;
        }
    }
}