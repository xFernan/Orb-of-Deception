using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Orb
{
    public class OnPlayerState : State
    {
        private readonly OrbController _orbController;
        private readonly Transform _transform;
        private readonly Transform _orbIdlePositionTransform;

        private readonly float _idleLerpPlayerFollowValue;
        private readonly float _idleFloatingMoveVelocity;
        private readonly float _idleFloatingMoveDistance;
        public OnPlayerState(OrbController orbController, float idleLerpPlayerFollowValue, float idleFloatingMoveVelocity, float idleFloatingMoveDistance)
        {
            _orbController = orbController;
            _transform = orbController.transform;
            _orbIdlePositionTransform = orbController.orbIdlePositionTransform;

            _idleLerpPlayerFollowValue = idleLerpPlayerFollowValue;
            _idleFloatingMoveVelocity = idleFloatingMoveVelocity;
            _idleFloatingMoveDistance = idleFloatingMoveDistance;
        }


        public override void Enter()
        {
            base.Enter();

            _orbController.CanBeThrown = true;
            _orbController.CanHit = false;
            _orbController.orbIdleParticles.Play();
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            
            var currentPosition = _transform.position;
            var orbIdlePosition = _orbIdlePositionTransform.position;

            var newPosition = Vector2.Lerp(currentPosition, orbIdlePosition, _idleLerpPlayerFollowValue);
                    
            newPosition.y += Mathf.Sin(Time.time * _idleFloatingMoveVelocity) *
                             _idleFloatingMoveDistance;
                    
            _transform.position = newPosition;
        }

        public override void Exit()
        {
            base.Exit();

            _orbController.CanBeThrown = false;
            _orbController.CanHit = true;
            _orbController.orbIdleParticles.Stop();
        }
    }
}