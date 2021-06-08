using UnityEngine;

namespace OrbOfDeception.Player
{
    public class GroundDetector
    {
        private readonly Transform[] _groundDetectors;
        private readonly float _groundDetectionRayDistance;
        
        private bool _isOnTheGround;

        public GroundDetector(Transform[] groundDetectors, float groundDetectionRayDistance)
        {
            _groundDetectors = groundDetectors;
            _groundDetectionRayDistance = groundDetectionRayDistance;
        }

        public void Update()
        {
            var isOnTheGround = false;
            foreach (var groundDetector in _groundDetectors)
            {
                isOnTheGround |= Physics2D.Raycast(groundDetector.position, Vector2.down, _groundDetectionRayDistance, LayerMask.GetMask("Ground"));
                if (isOnTheGround)
                    break;
            }

            _isOnTheGround = isOnTheGround;
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
    }
}