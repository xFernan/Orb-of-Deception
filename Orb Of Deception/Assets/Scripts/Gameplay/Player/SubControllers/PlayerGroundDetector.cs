using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerGroundDetector
    {
        private readonly Transform[] _groundDetectors;
        private readonly Transform _centralGroundDetector;
        private readonly float _groundDetectionRayDistance;
        private readonly float _coyoteTime;
        
        private bool _isOnTheGround;
        private float _coyoteTimeCounter;

        public PlayerGroundDetector(Transform[] groundDetectors, Transform centralGroundDetector, float groundDetectionRayDistance, float coyoteTime)
        {
            _groundDetectors = groundDetectors;
            _centralGroundDetector = centralGroundDetector;
            _groundDetectionRayDistance = groundDetectionRayDistance;
            _coyoteTime = coyoteTime;
        }

        public void Update(float deltaTime)
        {
            var isOnTheGroundCurrentFrame = CheckIfIsOnTheGround();

            if (_coyoteTimeCounter > 0)
            {
                _coyoteTimeCounter -= deltaTime;
            }
            
            var isOnTheGroundPreviousFrame = _isOnTheGround; 
            if (isOnTheGroundPreviousFrame && !isOnTheGroundCurrentFrame)
            {
                _coyoteTimeCounter = _coyoteTime;
            }
            
            if (!isOnTheGroundPreviousFrame && isOnTheGroundCurrentFrame)
            {
                if (GameManager.Player.isControlled) // PROVISIONAL
                {
                    GameManager.Player.soundsPlayer.Play("Landing");
                }
            }
            
            _isOnTheGround = isOnTheGroundCurrentFrame;
        }

        private bool CheckIfIsOnTheGround()
        {
            var isOnTheGroundCurrentFrame = false;
            
            foreach (var groundDetector in _groundDetectors)
            {
                var raycastHit = Physics2D.Raycast(groundDetector.position, Vector2.down, _groundDetectionRayDistance,
                    LayerMask.GetMask("Ground"));
                var hasCollided = raycastHit.collider != null;
                
                if (hasCollided)
                {
                    var fallingPlatformHit = raycastHit.collider.GetComponent<FallingPlatform>();
                    if (fallingPlatformHit != null)
                    {
                        hasCollided = GameManager.Player.Rigidbody.velocity.y <= 0.1f; // Mejorable?
                    }
                }
                
                isOnTheGroundCurrentFrame |= hasCollided;
            }

            return isOnTheGroundCurrentFrame;
        }
        
        public Vector3 GetGroundNormalVector()
        {
            var hit = Physics2D.Raycast(_centralGroundDetector.position, Vector2.down, _groundDetectionRayDistance, LayerMask.GetMask("Ground"));
            if (hit.collider == null)
                return Vector3.zero;
            else
                return hit.normal;
        }
        
        public void OnDrawGizmos()
        {
            if (_groundDetectors.Length == 0)
                return;

            foreach (var groundDetector in _groundDetectors)
            {
                var position = groundDetector.position;
                Gizmos.DrawLine(position, position + Vector3.down * _groundDetectionRayDistance);
            }
        }

        public bool IsOnTheGround()
        {
            return _isOnTheGround;
        }

        public bool IsOnCoyoteTime()
        {
            return _coyoteTimeCounter > 0;
        }
    }
}