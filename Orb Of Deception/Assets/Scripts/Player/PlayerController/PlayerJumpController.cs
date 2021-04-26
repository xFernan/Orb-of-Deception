using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerJumpController
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly PlayerGroundDetector _playerGroundDetector;
        private readonly float _jumpForce;
        private readonly float _jumpTime;
        
        private float _jumpTimeCounter;
        private bool _isJumping;

        public PlayerJumpController(Rigidbody2D rigidbody, float jumpForce, float jumpTime, PlayerGroundDetector playerGroundDetector)
        {
            _rigidbody = rigidbody;
            _playerGroundDetector = playerGroundDetector;
            _jumpForce = jumpForce;
            _jumpTime = jumpTime;
        }

        public void Jump()
        {
            if (!_isJumping && !_playerGroundDetector.IsOnTheGround())
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