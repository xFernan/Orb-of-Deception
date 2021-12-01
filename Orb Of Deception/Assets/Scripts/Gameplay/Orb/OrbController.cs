﻿using System.Collections;
using System.Collections.Generic;
using OrbOfDeception.Core;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Gameplay.Player;
using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Orb
{
    public class OrbController : GameEntity
    {
        #region Variables

        [Header("Particles variables")]
        public ParticleSystem directionAttackOrbParticles;
        public ParticleSystem orbIdleParticles;
        public TrailRenderer orbTrail;

        [SerializeField] private GameObject orbHitParticles;
        
        [Space]
        
        public GameObject bounceParticles;
        public GameObject ringParticles;
        public GameObject hitParticles;
        
        [Space]
        
        [SerializeField] private Color blackStateParticlesColor;
        [SerializeField] private Color whiteStateParticlesColor;
        
        [Header("Orb parameters")]
        public Transform orbIdlePositionTransform;
        [SerializeField] private float idleFloatingMoveDistance = 1;
        [SerializeField] private float idleFloatingMoveVelocity = 1;
        [SerializeField] private float idleVelocity = 0.5f;
        [SerializeField] private float radiusToGoIdle;
        
        [Space]
        
        [SerializeField] private float directionalAttackInitialForce;
        [SerializeField] private float directionalAttackDecelerationFactor = 0.95f;
        [SerializeField] private float directionalAttackMinVelocityToChangeState = 1;
        [SerializeField] private float directionalAttackFirstBounceVelocityBoost = 1.5f;

        [Space]
        
        [SerializeField] private float timeBetweenColorChange = 0.1f;
        [SerializeField] private float attractionForce;
        
        [Header("Script references")]
        
        [SerializeField] private OrbSpriteController orbSpriteController;
        [SerializeField] private OrbLightColorChanger orbLightColorChanger;
        [SerializeField] private MultipleParticlesController colorChangeParticles;
        
        private EntityColor _orbColor;
        private bool _hasReceivedAVelocityBoost;
        private int _ringParticlesCount;
        private bool _canChangeColor = true;
        
        public Rigidbody2D Rigidbody { get; private set; }
        public Collider2D PhysicsCollider { get; private set; }

        private OrbTrailController _orbTrailController;

        public bool CanBeThrown { get; set; }
        public bool CanHit { get; set; }
        
        public Color CurrentParticlesColor =>
            _orbColor == EntityColor.White ? whiteStateParticlesColor : blackStateParticlesColor;

        private FiniteStateMachine _stateMachine;
        private Dictionary<int, State> _states;
        
        public const int OnPlayerState = 0;
        public const int ReturningState = 1;
        public const int DirectionalAttackState = 2;
        #endregion

        #region Methods
        
        public void SetState(int stateId)
        {
            _stateMachine.SetState(_states[stateId]);
        }
        
        #region Monobehaviour Methods
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            PhysicsCollider = GetComponent<Collider2D>();
            
            _orbTrailController = new OrbTrailController(orbTrail);

            _stateMachine = new FiniteStateMachine();
            _states = new Dictionary<int, State>
            {
                {
                    OnPlayerState, new OnPlayerState(this, idleVelocity, idleFloatingMoveVelocity,
                        idleFloatingMoveDistance)
                },
                {
                    DirectionalAttackState, new DirectionalAttackState(this,
                        directionalAttackDecelerationFactor, directionalAttackMinVelocityToChangeState,
                        directionalAttackInitialForce, directionalAttackFirstBounceVelocityBoost)
                },
                {
                    ReturningState, new ReturningState(this, radiusToGoIdle, attractionForce)
                }
            };

            _stateMachine.SetInitialState(_states[OnPlayerState]);
            
            var inputManager = FindObjectOfType<InputManager>(); // Provisional
            inputManager.DirectionalAttack = DirectionalAttack;
            inputManager.Click = OnMouseClick;
            inputManager.ChangeOrbColor = ChangeColor;
        }

        private void Update()
        {
            _stateMachine?.Update(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate(Time.deltaTime);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            _stateMachine.OnCollisionEnter2D(other);
        }
        
        public void OnTriggerObjectInit(Collider2D colliderHit)
        {
            if (!CanHit) return;

            var objectHit = colliderHit.gameObject;
            var orbHittable = objectHit.GetComponent<IOrbHittable>();

            if (orbHittable == null) return;
            
            InstantiateOrbHitParticles(objectHit, colliderHit.ClosestPoint(transform.position));
            orbHittable.OnOrbHitEnter(_orbColor, 10/*PROVISIONAL*/);
            GameManager.Camera.Shake(0.2f, 0.1f);

            if (objectHit.layer != LayerMask.NameToLayer("Enemy")) return;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radiusToGoIdle);
        }
        
        #endregion
        
        private void OnMouseClick(Vector2 mousePosition)
        {
            if (!GameManager.Player.isControlled)
                return;
            
            if (CanBeThrown && !PauseController.Instance.IsPaused)
            {
                DirectionalAttack(GetDirectionFromOrbToMouse());
            }
        }

        private void InstantiateOrbHitParticles(GameObject hitEntity, Vector3 spawnPosition)
        {
            // Rehacer con pools.
            var particles = Instantiate(orbHitParticles, spawnPosition, Quaternion.identity).GetComponent<OrbHitParticlesController>();
            particles.Play(hitEntity);
        }
        
        private Vector3 GetDirectionFromOrbToMouse()
        {
            var worldPosition2D = GameManager.Camera.cameraComponent.ScreenToWorldPoint(Input.mousePosition);
            var position = transform.position;
            worldPosition2D.z = position.z;
            var direction = (worldPosition2D - position).normalized;
            
            return direction;
        }
        
        private void DirectionalAttack(Vector2 direction)
        {
            SetState(DirectionalAttackState);
            Rigidbody.velocity = directionalAttackInitialForce * direction;
        }
        
        private void ChangeColor()
        {
            if (!GameManager.Player.isControlled)
                return;
            
            if (!_canChangeColor) return;

            InitColorChangeDelay();
            
            _orbColor = _orbColor switch
            {
                EntityColor.Black => EntityColor.White,
                EntityColor.White => EntityColor.Black,
                _ => _orbColor
            };

            var directionalAttackParticles = directionAttackOrbParticles.main;
            var idleParticles = orbIdleParticles.main;

            var particlesColor = CurrentParticlesColor;
            
            orbLightColorChanger.ChangeLightColor(_orbColor);
            orbSpriteController.SetOrbSprite(_orbColor);
            
            directionalAttackParticles.startColor = particlesColor;
            idleParticles.startColor = particlesColor;
            colorChangeParticles.SetColor(particlesColor);
            _orbTrailController.SetTrailColor(particlesColor);
            
            colorChangeParticles.Play();
        }

        private void InitColorChangeDelay()
        {
            _canChangeColor = false;
            StartCoroutine(CanChangeColorAgainCoroutine());
        }

        private IEnumerator CanChangeColorAgainCoroutine()
        {
            yield return new WaitForSeconds(timeBetweenColorChange);

            _canChangeColor = true;
        }

        public EntityColor GetColor()
        {
            return _orbColor;
        }
        
        #region Particles Methods
        
        public void SpawnHitParticles(GameObject objectHit)
        {
            var particlesObject = Instantiate(hitParticles, objectHit.transform.position, Quaternion.identity); // Provisional.

            var particlesController = particlesObject.GetComponent<MultipleParticlesController>();
            particlesController.Play(CurrentParticlesColor);

            var particlesFollower = particlesObject.GetComponent<ObjectFollower>();
            particlesFollower.SetTarget(objectHit.transform);
        }
        
        #endregion
        
        #endregion
    }
}
