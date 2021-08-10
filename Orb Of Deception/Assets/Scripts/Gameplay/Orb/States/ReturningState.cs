using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Orb
{
    public class ReturningState : State
    {
        private readonly OrbController _orbController;
        private readonly Transform _transform;
        private readonly Transform _orbIdlePositionTransform;
        private readonly Rigidbody2D _rigidbody;

        private readonly float _radiusToGoIdle;
        private readonly float _attractionForce;
        public ReturningState(OrbController orbController, float radiusToGoIdle, float attractionForce)
        {
            _orbController = orbController;
            _transform = orbController.transform;
            _orbIdlePositionTransform = orbController.orbIdlePositionTransform;
            _rigidbody = orbController.Rigidbody;

            _radiusToGoIdle = radiusToGoIdle;
            _attractionForce = attractionForce;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            if (Vector2.Distance(_transform.position, _orbIdlePositionTransform.position) <= _radiusToGoIdle)
            {
                _orbController.SetState(OrbController.OnPlayerState);
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            
            var velocityValue = _rigidbody.velocity.magnitude;
            var direction = (_orbIdlePositionTransform.position - _transform.position).normalized;
            _rigidbody.velocity = direction * velocityValue;
            _rigidbody.AddForce(direction * _attractionForce);
        }

        public override void Exit()
        {
            base.Exit();
            
            _rigidbody.velocity = Vector2.zero;
            _orbController.directionAttackOrbParticles.Stop();
        }
    }
}