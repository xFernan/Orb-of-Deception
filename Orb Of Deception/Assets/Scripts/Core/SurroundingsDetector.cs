using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public class SurroundingsDetector : MonoBehaviour
    {
        #region Variables
        
        [Header("Ground Detection Variables")]
        [SerializeField] private float groundDetectorRange;
        [SerializeField] private Transform leftGroundDetector;
        [SerializeField] private Transform rightGroundDetector;
        [SerializeField] private bool isDetectingFlatGroundOnly = false;
        
        [Space]
        
        [Header("Wall Detection Variables")]
        [SerializeField] private float wallDetectorRange;
        [SerializeField] private Transform[] leftWallDetectors;
        [SerializeField] private Transform[] rightWallDetectors;

        public bool IsDetectingRightWall { get; private set; }
        public bool IsDetectingLeftWall { get; private set; }
        public bool IsDetectingRightGround { get; private set; }
        public bool IsDetectingLeftGround { get; private set; }
        public bool IsDetectingGround => IsDetectingRightGround || IsDetectingLeftGround;
        
        #endregion
        
        #region Methods
        
        #region MonoBehaviour Methods
        
        private void Update()
        {
            IsDetectingRightWall = IsRaycastingToLayer(rightWallDetectors, Vector2.right, wallDetectorRange,
                LayerMask.GetMask("Ground"));
            
            IsDetectingLeftWall = IsRaycastingToLayer(leftWallDetectors, Vector2.left, wallDetectorRange,
                LayerMask.GetMask("Ground"));
            
            IsDetectingRightGround = isDetectingFlatGroundOnly ? IsRaycastingToLayerFlat(rightGroundDetector, Vector2.down, groundDetectorRange,
                LayerMask.GetMask("Ground")) :
                IsRaycastingToLayer(rightGroundDetector, Vector2.down, groundDetectorRange,
                LayerMask.GetMask("Ground"));
            
            IsDetectingLeftGround = isDetectingFlatGroundOnly ? IsRaycastingToLayerFlat(leftGroundDetector, Vector2.down, groundDetectorRange,
                LayerMask.GetMask("Ground")) :
                IsRaycastingToLayer(leftGroundDetector, Vector2.down, groundDetectorRange,
                LayerMask.GetMask("Ground"));
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (leftGroundDetector != null)
            {
                var leftGroundDetectorPosition = leftGroundDetector.position;
                Gizmos.DrawLine(leftGroundDetectorPosition, leftGroundDetectorPosition + Vector3.down * groundDetectorRange);
            }
            if (rightGroundDetector != null)
            {
                var rightGroundDetectorPosition = rightGroundDetector.position;
                Gizmos.DrawLine(rightGroundDetectorPosition, rightGroundDetectorPosition + Vector3.down * groundDetectorRange);
            }
            
            Gizmos.color = Color.green;
            foreach (var rightWallDetector in rightWallDetectors)
            {
                var rightWallDetectorPosition = rightWallDetector.position;
                Gizmos.DrawLine(rightWallDetectorPosition, rightWallDetectorPosition + Vector3.right * wallDetectorRange);
            }
            foreach (var leftWallDetector in leftWallDetectors)
            {
                var leftWallDetectorPosition = leftWallDetector.position;
                Gizmos.DrawLine(leftWallDetectorPosition, leftWallDetectorPosition + Vector3.left * wallDetectorRange);
            }
        }
        
        #endregion
        
        private static bool IsRaycastingToLayer(IEnumerable<Transform> origins, Vector2 rayDirection, float maxDistance, LayerMask layerMask)
        {
            return origins.Any(origin => Physics2D.Raycast(origin.position, rayDirection, maxDistance, layerMask));
        }
        
        private static bool IsRaycastingToLayerFlat(Transform origin, Vector2 rayDirection, float maxDistance, LayerMask layerMask)
        {
            var raycastHit = Physics2D.Raycast(origin.position, rayDirection, maxDistance, layerMask);
            //Debug.Log((raycastHit.normal + " == " + Vector2.up) + " || " + (raycastHit.normal == Vector2.up));
            return raycastHit.collider != null && Vector3.Distance(raycastHit.normal, Vector2.up) < 0.1f;
        }
        
        private static bool IsRaycastingToLayer(Transform origin, Vector2 rayDirection, float maxDistance, LayerMask layerMask)
        {
            return Physics2D.Raycast(origin.position, rayDirection, maxDistance, layerMask);
        }
        
        #endregion
    }
}
