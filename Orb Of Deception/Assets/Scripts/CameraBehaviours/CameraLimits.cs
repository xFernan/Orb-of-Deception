using System;
using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraLimits : MonoBehaviour
    {
        [SerializeField] private bool isDrawingOnGizmos = false;
        [SerializeField] private Color gizmosColor = Color.magenta;
        [SerializeField] protected Transform limitSouthWest;
        [SerializeField] protected Transform limitNorthEast;

        private void OnDrawGizmos()
        {
            if (!isDrawingOnGizmos || limitNorthEast == null || limitSouthWest == null)
            {
                return;
            }
            
            var camera = Camera.main;
            float height, width;
            if (camera == null)
            {
                height = 11.25f;//camera.orthographicSize * 2.0f;
                width = 20;//height * camera.aspect;
            }
            else
            {
                height = camera.orthographicSize * 2.0f;
                width = height * camera.aspect;
            }
            
            var limitSouthWestPosition = (Vector2) limitSouthWest.position + new Vector2(-width, -height) / 2;
            var limitNorthEastPosition = (Vector2) limitNorthEast.position + new Vector2(width, height) / 2;
            var limitSouthEastPosition = new Vector2(limitNorthEastPosition.x, limitSouthWestPosition.y);
            var limitNorthWestPosition = new Vector2(limitSouthWestPosition.x, limitNorthEastPosition.y);
            
            Gizmos.color = gizmosColor;
            
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

        public Vector3 GetSouthWestLimit()
        {
            return limitSouthWest.position;
        }

        public Vector3 GetNorthEastLimit()
        {
            return limitNorthEast.position;
        }
    }
}
