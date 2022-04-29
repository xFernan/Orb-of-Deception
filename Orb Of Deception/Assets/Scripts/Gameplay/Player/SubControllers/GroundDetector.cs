using UnityEngine;

namespace OrbOfDeception.Player
{
    public class GroundDetector
    {
        private readonly Transform[] _groundDetectors;
        private readonly Transform _centralGroundDetector;
        private readonly float _groundDetectionRayDistance;
        private readonly float _coyoteTime;
        
        private bool _isOnTheGround;
        private float _coyoteTimeCounter;

        public GroundDetector(Transform[] groundDetectors, Transform centralGroundDetector, float groundDetectionRayDistance, float coyoteTime)
        {
            _groundDetectors = groundDetectors;
            _centralGroundDetector = centralGroundDetector;
            _groundDetectionRayDistance = groundDetectionRayDistance;
            _coyoteTime = coyoteTime;
        }

        public void Update(float deltaTime)
        {
            var isOnTheGroundCurrentFrame = false;
            foreach (var groundDetector in _groundDetectors)
            {
                isOnTheGroundCurrentFrame |= Physics2D.Raycast(groundDetector.position, Vector2.down, _groundDetectionRayDistance, LayerMask.GetMask("Ground"));
                if (isOnTheGroundCurrentFrame)
                    break;
            }

            if (_coyoteTimeCounter > 0)
            {
                _coyoteTimeCounter -= deltaTime;
            }
            
            var isOnTheGroundPreviousFrame = _isOnTheGround; 
            if (isOnTheGroundPreviousFrame && !isOnTheGroundCurrentFrame)
            {
                _coyoteTimeCounter = _coyoteTime;
            }
            else if (!isOnTheGroundPreviousFrame && isOnTheGroundCurrentFrame)
            {
                GameManager.Player.SoundsPlayer.Play("Landing");
            }
            
            _isOnTheGround = isOnTheGroundCurrentFrame;
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