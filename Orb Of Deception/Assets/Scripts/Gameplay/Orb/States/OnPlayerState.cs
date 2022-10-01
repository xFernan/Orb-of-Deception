using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Orb
{
    public class OnPlayerState : State
    {
        private readonly OrbController _orbController;
        private readonly Transform _transform;
        private readonly Transform _orbIdlePositionTransform;

        private readonly float _idlePlayerFollowSmoothTime;
        private readonly float _idleFloatingMoveVelocity;

        private Vector2 _speed = Vector2.zero;
        
        public OnPlayerState(OrbController orbController, float idlePlayerFollowSmoothTime, float idleFloatingMoveVelocity)
        {
            _orbController = orbController;
            _transform = orbController.transform;
            _orbIdlePositionTransform = orbController.orbIdlePositionTransform;

            _idlePlayerFollowSmoothTime = idlePlayerFollowSmoothTime;
            _idleFloatingMoveVelocity = idleFloatingMoveVelocity;
        }

        public override void Enter()
        {
            base.Enter();
            
            _orbController.CanBeThrown = true;
            _orbController.CanHit = false;
            _orbController.orbIdleParticles.Play();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            if (Time.timeScale == 0) return;
            
            var currentPosition = _transform.position;
            var orbIdlePosition = _orbIdlePositionTransform.position;

            var newPosition = Vector2.SmoothDamp(currentPosition, orbIdlePosition, ref _speed, _idlePlayerFollowSmoothTime);
            
            //var newPosition = Vector2.Lerp(currentPosition, orbIdlePosition, _idleLerpPlayerFollowValue * Time.deltaTime);
                    
            newPosition.y += Mathf.Sin(Time.time * 8) * _idleFloatingMoveVelocity * Time.deltaTime; // Need fix.
            //newPosition.y += PeriodicLinearFunction(Time.time * 6) * _idleFloatingMoveVelocity * Time.deltaTime;
            _transform.position = newPosition;
        }

        /*private float PeriodicLinearFunction(float value)
        {
            var rest = value % 4;
            return Mathf.Abs(rest - 2) - 1;
        }*/
        
        public override void Exit()
        {
            base.Exit();

            _orbController.CanBeThrown = false;
            _orbController.CanHit = true;
            _orbController.orbIdleParticles.Stop();
        }
    }
}