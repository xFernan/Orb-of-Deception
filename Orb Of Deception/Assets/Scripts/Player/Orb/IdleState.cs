using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Player.Orb
{
    public class IdleState : State
    {
        
        private readonly Transform _transform;
        private readonly Transform _target;
        
        public IdleState(Transform transform, Transform target)
        {
            _transform = transform;
            _target = target;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            _transform.position = _target.position;
        }
    }
}