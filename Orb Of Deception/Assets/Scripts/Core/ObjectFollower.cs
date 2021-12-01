using UnityEngine;

namespace OrbOfDeception.Core
{
    public class ObjectFollower : MonoBehaviour
    {
        private Transform _target;
        
        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }

        private void Update()
        {
            if (_target == null) return;
            
            transform.position = _target.position;
        }
    }
}
