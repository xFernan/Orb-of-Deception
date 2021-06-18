using UnityEngine;

namespace OrbOfDeception.Player
{
    public class JumpController
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly GroundDetector _groundDetector;
        private readonly float _jumpForce;
        private readonly float _jumpTime;
        
        private float _jumpTimeCounter;
        private bool _isJumping;

        public JumpController(Rigidbody2D rigidbody, float jumpForce, float jumpTime, GroundDetector groundDetector)
        {
            _rigidbody = rigidbody;
            _groundDetector = groundDetector;
            _jumpForce = jumpForce;
            _jumpTime = jumpTime;
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
            if (!_isJumping)
            {
                return;
            }
            
            var newVelocity = _rigidbody.velocity;
            newVelocity.y = _jumpForce;
            _rigidbody.velocity = newVelocity;
            
            _jumpTimeCounter += Time.deltaTime;
            if (_jumpTimeCounter >= _jumpTime)
            {
                _isJumping = false;
            }
        }
    }
}