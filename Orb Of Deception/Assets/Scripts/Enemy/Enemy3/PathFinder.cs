using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] private float nextWaypointDistance = 0.5f;
        [SerializeField] private float timeBetweenPathUpdate = 0.2f;
        
        private Seeker _seeker;

        private Transform _target;
        private Path _path;
        private Coroutine _updatePathCoroutine;

        private int _currentWaypoint = 0;
        private Vector2 _direction;
        private bool _isFindingPath = false;
        
        private void Awake()
        {
            _seeker = GetComponent<Seeker>();
        }

        private void FixedUpdate()
        {
            if (!_isFindingPath || _target == null || _path == null || _currentWaypoint >= _path.vectorPath.Count)
            {
                return;
            }

            var position = transform.position;
            var distance = Vector2.Distance(position, _path.vectorPath[_currentWaypoint]);
            _direction = (_path.vectorPath[_currentWaypoint] - position).normalized;
            
            if (distance < nextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }

        public void StartFindingPath()
        {
            _isFindingPath = true;
            _updatePathCoroutine = StartCoroutine(UpdatePathCoroutine());
        }
        
        public void StopFindingPath()
        {
            _isFindingPath = false;
            StopCoroutine(_updatePathCoroutine);
        }
        
        private IEnumerator UpdatePathCoroutine()
        {
            _seeker.StartPath(transform.position, _target.position, OnPathComplete);
            yield return new WaitForSeconds(timeBetweenPathUpdate);
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentWaypoint = 0;
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        public Vector2 GetCurrentPathDirection()
        {
            return _direction;
        }

        public float GetCurrentPathLength()
        {
            return _path.GetTotalLength();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
        }
    }
}