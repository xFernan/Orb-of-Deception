using System.Collections;
using OrbOfDeception.Patterns;
using OrbOfDeception.Player;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception.Orb
{
    public class DirectionalAttackState : State
    {
        private readonly OrbController _orbController;
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly Collider2D _physicsCollider;

        private readonly float _directionalAttackDecelerationFactor;
        private readonly float _directionalAttackMinVelocityToChangeState;
        private readonly float _directionalAttackInitialForce;
        private readonly float _directionalAttackFirstBounceVelocityBoost;
        
        private int _ringParticlesCount;
        private bool _hasReceivedAVelocityBoost;
        
        public DirectionalAttackState(OrbController orbController, float directionalAttackDecelerationFactor, float directionalAttackMinVelocityToChangeState,
            float directionalAttackInitialForce, float directionalAttackFirstBounceVelocityBoost)
        {
            _orbController = orbController;
            _transform = orbController.transform;
            _rigidbody = orbController.Rigidbody;
            _physicsCollider = orbController.PhysicsCollider;

            _directionalAttackDecelerationFactor = directionalAttackDecelerationFactor;
            _directionalAttackMinVelocityToChangeState = directionalAttackMinVelocityToChangeState;
            _directionalAttackInitialForce = directionalAttackInitialForce;
            _directionalAttackFirstBounceVelocityBoost = directionalAttackFirstBounceVelocityBoost;
        }

        public override void Enter()
        {
            base.Enter();

            _ringParticlesCount = 0;
            _hasReceivedAVelocityBoost = false;
            _physicsCollider.enabled = true;
            _orbController.orbDirectionalAttackParticles.Play();
            SpawnRingParticles();
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            
            _rigidbody.velocity *= _directionalAttackDecelerationFactor * (SaveSystem.currentMaskType == PlayerMaskController.MaskType.ScarletMask ? 0.9f : 1);
            
            if (_rigidbody.velocity.magnitude <= _directionalAttackMinVelocityToChangeState)
            {
                _orbController.SetState(OrbController.ReturningState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            _physicsCollider.enabled = false;
        }

        public override void OnCollisionEnter2D(Collision2D other)
        {
            base.OnCollisionEnter2D(other);
            
            var orbCollisionable = other.gameObject.GetComponent<IOrbCollisionable>();
            orbCollisionable?.OnOrbCollisionEnter();
            
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
            
            SpawnBounceParticles(other.contacts[0].point, _orbController.CurrentParticlesColor);

            if (_hasReceivedAVelocityBoost)
                return;
            
            var otherColliderEffector = other.gameObject.GetComponent<PlatformEffector2D>();
            if (otherColliderEffector != null)
                return;
            
            ApplyVelocityBoost();

            _hasReceivedAVelocityBoost = true;
        }

        private void ApplyVelocityBoost()
        {
            var oldVelocity = _rigidbody.velocity;
            var newVelocity = oldVelocity.magnitude + _directionalAttackFirstBounceVelocityBoost
                * (SaveSystem.currentMaskType == PlayerMaskController.MaskType.ScarletMask ? 1.9f : 1);;
            _rigidbody.velocity = oldVelocity.normalized * Mathf.Min(newVelocity, _directionalAttackInitialForce);

            if (_ringParticlesCount != 3) return;
            
            //_orbController.StartCoroutine(SpawnRingParticlesCoroutine(0.02f, 6));
            //_orbController.StartCoroutine(SpawnRingParticlesCoroutine(0.08f, 9));
        }

        private void SpawnBounceParticles(Vector2 particlesPosition, Color particlesColor)
        {
            var bounceParticlesObject = Object.Instantiate(_orbController.bounceParticles, particlesPosition, Quaternion.identity); // Cambiar por Object Pool.
            var bounceParticlesMain = bounceParticlesObject.GetComponent<ParticleSystem>().main;

            Camera.main.WorldToScreenPoint(particlesPosition);
            bounceParticlesMain.startColor = particlesColor;
        }
        
        private IEnumerator SpawnRingParticlesCoroutine(float time, float speed)
        {
            yield return new WaitForSeconds(time);
            _ringParticlesCount++;
            SpawnRingParticles(speed);
        }
        
        private void SpawnRingParticles(float speed)
        {
            var ringParticlesObject = (GameObject) Object.Instantiate(_orbController.ringParticles, _transform.position, Quaternion.LookRotation(_rigidbody.velocity.normalized)); // Cambiar por Object Pool.
            var ringParticlesMain = ringParticlesObject.GetComponent<ParticleSystem>().main;
            ringParticlesMain.startSpeed = speed;
            ringParticlesMain.startColor = _orbController.CurrentParticlesColor;
        }
        
        private void SpawnRingParticles()
        {
            _orbController.StartCoroutine(SpawnRingParticlesCoroutine(0.02f, 3));
            _orbController.StartCoroutine(SpawnRingParticlesCoroutine(0.07f, 6));
            _orbController.StartCoroutine(SpawnRingParticlesCoroutine(0.15f, 9));
        }
    }
}