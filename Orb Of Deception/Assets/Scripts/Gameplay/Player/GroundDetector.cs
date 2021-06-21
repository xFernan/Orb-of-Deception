using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class GroundDetector
    {
        private readonly Transform[] _groundDetectors;
        private readonly float _groundDetectionRayDistance;
        private readonly float _coyoteTime;
        
        private bool _isOnTheGround;
        private float _coyoteTimeCounter;

        public GroundDetector(Transform[] groundDetectors, float groundDetectionRayDistance, float coyoteTime)
        {
            _groundDetectors = groundDetectors;
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
            
            _isOnTheGround = isOnTheGroundCurrentFrame;
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