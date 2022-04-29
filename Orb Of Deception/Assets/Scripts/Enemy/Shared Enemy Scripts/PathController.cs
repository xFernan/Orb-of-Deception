using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private Transform[] pathWaypoints;

        private int currentWaypoint = 0;

        private void OnDrawGizmos()
        {
            if (pathWaypoints.IsNullOrEmpty()) return;
            
            Gizmos.color = Color.green;
            var pointSphereRadius = 0.1f;
            
            for (int i = 0; i < pathWaypoints.Length - 1; i++)
            {
                if (pathWaypoints[i] == null)
                    continue;
                var pointFromPosition = pathWaypoints[i].position;
                var pointToPosition = pathWaypoints[i + 1].position;
                Gizmos.DrawSphere(pointFromPosition, pointSphereRadius);
                Gizmos.DrawLine(pointFromPosition, pointToPosition);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pathWaypoints[0].position, pointSphereRadius);
            
            Gizmos.color = Color.green;
            var pathWaypointPosition = pathWaypoints.Last().position;
            Gizmos.DrawSphere(pathWaypointPosition, pointSphereRadius);
            Gizmos.DrawLine(pathWaypointPosition, pathWaypoints[0].position);
        }

        public void GoToNextWaypoint()
        {
            currentWaypoint++;
            currentWaypoint %= pathWaypoints.Length;
        }

        public Vector3 GetCurrentWaypointPosition()
        {
            return pathWaypoints[currentWaypoint].position;
        }
    }
}
