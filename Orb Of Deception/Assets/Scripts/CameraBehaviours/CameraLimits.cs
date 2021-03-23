using UnityEngine;

namespace Nanref.CameraBehaviours
{
    public class CameraLimits : MonoBehaviour
    {
        [SerializeField] private Transform limitSouthWest;
        [SerializeField] private Transform limitNorthEast;

        private void OnDrawGizmos()
        {
            if (limitNorthEast == null || limitSouthWest == null)
            {
                return;
            }
            
            var limitSouthWestPosition = (Vector2) limitSouthWest.position;
            var limitNorthEastPosition = (Vector2) limitNorthEast.position;
            var limitSouthEastPosition = new Vector2(limitNorthEastPosition.x, limitSouthWestPosition.y);
            var limitNorthWestPosition = new Vector2(limitSouthWestPosition.x, limitNorthEastPosition.y);
            
            Gizmos.color = Color.magenta;
            
            Gizmos.DrawLine(limitSouthWestPosition, limitSouthEastPosition);
            Gizmos.DrawLine(limitNorthEastPosition, limitNorthWestPosition);
            Gizmos.DrawLine(limitSouthEastPosition, limitNorthEastPosition);
            Gizmos.DrawLine(limitSouthWestPosition, limitNorthWestPosition);
        }

        public float GetMinX()
        {
            return limitSouthWest.position.x;
        }
        
        public float GetMaxX()
        {
            return limitNorthEast.position.x;
        }

        public float GetMinY()
        {
            return limitSouthWest.position.y;
        }
        
        public float GetMaxY()
        {
            return limitNorthEast.position.y;
        }
        
    }
}
