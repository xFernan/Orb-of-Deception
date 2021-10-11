using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraLimits : MonoBehaviour
    {
        [SerializeField] private Transform limitSouthWest;
        [SerializeField] private Transform limitNorthEast;

        private void OnDrawGizmos()
        {
            DrawLimits();
        }

        public void DrawLimits()
        {
            if (limitNorthEast == null || limitSouthWest == null)
            {
                return;
            }

            var camera = Camera.main;
            var height = camera.orthographicSize * 2.0f;
            var width = height * camera.aspect;
            
            var limitSouthWestPosition = (Vector2) limitSouthWest.position + new Vector2(-width, -height) / 2;
            var limitNorthEastPosition = (Vector2) limitNorthEast.position + new Vector2(width, height) / 2;
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
