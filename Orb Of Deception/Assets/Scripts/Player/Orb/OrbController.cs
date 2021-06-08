﻿using OrbOfDeception.Core;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Enemy;
using UnityEngine;

namespace OrbOfDeception.Player.Orb
{
    public sealed class OrbController : GameEntity
    {
        #region Variables

        private enum OrbState
        {
            OnPlayer,
            Returning,
            DirectionalAttack
        }

        private OrbState _state;

        [SerializeField] private GameObject bounceParticles;
        [SerializeField] private Transform orbIdlePositionTransform;
        [SerializeField] private ParticleSystem directionAttackOrbParticles;
        [SerializeField] private ParticleSystem orbIdleParticles;
        [SerializeField] private ParticleSystem orbColorChangeParticles;
        [SerializeField] private float directionalAttackInitialForce;
        [SerializeField] private float attractionForce;
        [SerializeField] private float radiusToGoIdle;
        [SerializeField] private float idleFloatingMoveDistance = 1;
        [SerializeField] private float idleFloatingMoveVelocity = 1;
        [SerializeField] private float idleLerpPlayerFollowValue = 0.5f;
        [SerializeField] private float directionalAttackDecelerationFactor = 0.95f;
        [SerializeField] private float directionalAttackMinVelocityToChangeState = 1;
        [SerializeField] private float directionalAttackFirstBounceVelocityBoost = 1.5f;

        private InputManager _inputManager;
        private EntityColor _orbColor;
        private bool _hasReceivedAVelocityBoost;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _physicsCollider;
        
        #endregion

        #region Methods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _inputManager = FindObjectOfType<InputManager>();
            _physicsCollider = GetComponent<Collider2D>();
            
            _physicsCollider.enabled = false;
            _state = OrbState.OnPlayer;
            _orbColor = EntityColor.White;
            _hasReceivedAVelocityBoost = false;
            directionAttackOrbParticles.Stop();

            _inputManager.DirectionalAttack = DirectionalAttack;
            _inputManager.Click = OnMouseClick;
            _inputManager.ChangeOrbColor = ChangeColor;
        }

        private void Update()
        {
            switch (_state)
            {
                case OrbState.Returning:
                    if (Vector2.Distance(transform.position, orbIdlePositionTransform.position) <= radiusToGoIdle)
                    {
                        _rigidbody.velocity = Vector2.zero;
                        orbIdleParticles.Play();
                        directionAttackOrbParticles.Stop();
                        _state = OrbState.OnPlayer;
                    }
                    break;
            }
        }

        private void OnMouseClick(Vector2 mousePosition)
        {
            if (_state == OrbState.OnPlayer)
            {
                DirectionalAttack(GetDirectionFromOrbToMouse());
            }
        }

        private Vector3 GetDirectionFromOrbToMouse()
        {
            var worldPosition2D = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            worldPosition2D.z = transform.position.z;
            var direction = (worldPosition2D - transform.position).normalized;
            
            return direction;
        }
        
        private void DirectionalAttack(Vector2 direction)
        {
            if (_state != OrbState.OnPlayer) return;
            
            _physicsCollider.enabled = true;
            _hasReceivedAVelocityBoost = false;
            _state = OrbState.DirectionalAttack;
            orbIdleParticles.Stop();
            directionAttackOrbParticles.Play();
            _rigidbody.velocity = directionalAttackInitialForce * direction;
        }
        
        private void FixedUpdate()
        {
            switch (_state)
            {
                case OrbState.OnPlayer:
                    var currentPosition = transform.position;
                    var orbIdlePosition = orbIdlePositionTransform.position;

                    var newPosition = Vector2.Lerp(currentPosition, orbIdlePosition, idleLerpPlayerFollowValue);
                    
                    newPosition.y += Mathf.Sin(Time.time * idleFloatingMoveVelocity) *
                        idleFloatingMoveDistance;
                    
                    transform.position = newPosition;
                    
                    break;
                case OrbState.DirectionalAttack:
                    //_rigidbody.AddForce(-_rigidbody.velocity.normalized * attractionForce);
                    _rigidbody.velocity = _rigidbody.velocity * directionalAttackDecelerationFactor;
                    if (_rigidbody.velocity.magnitude <= directionalAttackMinVelocityToChangeState)
                    {
                        _physicsCollider.enabled = false;
                        _state = OrbState.Returning;
                    }
                    break;
                case OrbState.Returning:
                    var velocityValue = _rigidbody.velocity.magnitude;
                    var direction = (orbIdlePositionTransform.position - transform.position).normalized;
                    _rigidbody.velocity = direction * velocityValue;
                    _rigidbody.AddForce(direction * attractionForce);
                    break;
            }
        }

        private void ChangeColor()
        {
            if (_orbColor == EntityColor.Black)
                _orbColor = EntityColor.White;
            else if (_orbColor == EntityColor.White)
                _orbColor = EntityColor.Black;
            
            var directionalAttackParticles = directionAttackOrbParticles.main;
            var idleParticles = orbIdleParticles.main;
            var colorChangeParticles = orbColorChangeParticles.main;
            
            if (_orbColor == EntityColor.White)
            {
                _spriteRenderer.color = Color.white;
                directionalAttackParticles.startColor = Color.white;
                idleParticles.startColor = Color.white;
                colorChangeParticles.startColor = Color.white;
            }
            else if (_orbColor == EntityColor.Black)
            {
                _spriteRenderer.color = new Color(0.1f, 0.1f, 0.1f);
                directionalAttackParticles.startColor = Color.black;
                idleParticles.startColor = Color.black;
                colorChangeParticles.startColor = Color.black;
            }
            
            orbColorChangeParticles.Play();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radiusToGoIdle);
        }

        public void OnTriggerObjectInit(GameObject hittableObject)
        {
            if (_state == OrbState.OnPlayer) return;
            
            var orbHittable = hittableObject.GetComponent<IOrbHittable>();

            orbHittable?.Hit(_orbColor, 10/*PROVISIONAL*/);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
            
            SpawnBounceParticles(other.contacts[0].point, _orbColor == EntityColor.White ? Color.white : new Color(0.1f, 0.1f, 0.1f));
            
            if (_state != OrbState.DirectionalAttack || _hasReceivedAVelocityBoost)
                return;
            
            var newVelocity = _rigidbody.velocity.magnitude + directionalAttackFirstBounceVelocityBoost;
            var oldVelocity = _rigidbody.velocity;
            _rigidbody.velocity = _rigidbody.velocity.normalized * Mathf.Min(newVelocity, directionalAttackInitialForce);
            
            _hasReceivedAVelocityBoost = true;
        }

        private void SpawnBounceParticles(Vector2 particlesPosition, Color particlesColor)
        {
            var bounceParticlesObject = Instantiate(bounceParticles, particlesPosition, Quaternion.identity); // Cambiar por Object Pool.
            var bounceParticlesMain = bounceParticlesObject.GetComponent<ParticleSystem>().main;

            Camera.main.WorldToScreenPoint(particlesPosition);
            bounceParticlesMain.startColor = particlesColor;
        }
        
        #endregion
    }
}
