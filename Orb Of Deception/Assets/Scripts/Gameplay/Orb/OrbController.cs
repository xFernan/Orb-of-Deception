using System;
using System.Collections;
using OrbOfDeception.Core;
using OrbOfDeception.Core.Input;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Orb
{
    public class OrbController : GameEntity
    {
        #region Variables

        private enum OrbState
        {
            OnPlayer,
            Returning,
            DirectionalAttack
        }

        private OrbState _state;

        [Header("Particles variables")]
        [SerializeField] private ParticleSystem directionAttackOrbParticles;
        [SerializeField] private ParticleSystem orbIdleParticles;
        [SerializeField] private TrailRenderer orbTrail;
        
        [Space]
        
        [SerializeField] private GameObject bounceParticles;
        [SerializeField] private GameObject ringParticles;
        [SerializeField] private GameObject hitParticles;
        
        [Space]
        
        [SerializeField] private Color blackStateParticlesColor;
        [SerializeField] private Color whiteStateParticlesColor;
        
        [Header("Orb parameters")]
        [SerializeField] private Transform orbIdlePositionTransform;
        [SerializeField] private float idleFloatingMoveDistance = 1;
        [SerializeField] private float idleFloatingMoveVelocity = 1;
        [SerializeField] private float idleLerpPlayerFollowValue = 0.5f;
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
        
        private Rigidbody2D _rigidbody;
        private Collider2D _physicsCollider;

        private InputManager _inputManager;
        private OrbTrailController _orbTrailController;

        private Color CurrentParticlesColor =>
            _orbColor == EntityColor.White ? whiteStateParticlesColor : blackStateParticlesColor;
        #endregion

        #region Methods

        #region Monobehaviour Methods
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _physicsCollider = GetComponent<Collider2D>();
            
            _inputManager = FindObjectOfType<InputManager>(); // Provisional
            
            _orbTrailController = new OrbTrailController(orbTrail);
            
            _state = OrbState.OnPlayer;
            _orbColor = EntityColor.White;
            _physicsCollider.enabled = false;

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
                case OrbState.OnPlayer:
                    break;
                case OrbState.DirectionalAttack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                    _rigidbody.velocity = _rigidbody.velocity * directionalAttackDecelerationFactor;
                    if (_rigidbody.velocity.magnitude <= directionalAttackMinVelocityToChangeState)
                    {
                        _physicsCollider.enabled = false;
                        _ringParticlesCount = 0;
                        _state = OrbState.Returning;
                    }
                    break;
                case OrbState.Returning:
                    var velocityValue = _rigidbody.velocity.magnitude;
                    var direction = (orbIdlePositionTransform.position - transform.position).normalized;
                    _rigidbody.velocity = direction * velocityValue;
                    _rigidbody.AddForce(direction * attractionForce);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
            
            SpawnBounceParticles(other.contacts[0].point, CurrentParticlesColor);

            if (_state != OrbState.DirectionalAttack || _hasReceivedAVelocityBoost)
                return;
            
            var oldVelocity = _rigidbody.velocity;
            var newVelocity = oldVelocity.magnitude + directionalAttackFirstBounceVelocityBoost;
            _rigidbody.velocity = oldVelocity.normalized * Mathf.Min(newVelocity, directionalAttackInitialForce);

            if (_ringParticlesCount == 3)
            {
                StartCoroutine(SpawnRingParticlesCoroutine(0.02f, 3));
                StartCoroutine(SpawnRingParticlesCoroutine(0.08f, 6));
            }
            
            _hasReceivedAVelocityBoost = true;
        }
        
        public void OnTriggerObjectInit(GameObject objectHit)
        {
            if (_state == OrbState.OnPlayer) return;
            
            var orbHittable = objectHit.GetComponent<IOrbHittable>();

            if (orbHittable == null) return;
            
            orbHittable.Hit(_orbColor, 10/*PROVISIONAL*/);

            if (objectHit.layer == LayerMask.NameToLayer("Player")) return;
            
            SpawnHitParticles(objectHit);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radiusToGoIdle);
        }
        
        #endregion
        
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
            var position = transform.position;
            worldPosition2D.z = position.z;
            var direction = (worldPosition2D - position).normalized;
            
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
            
            SpawnRingParticles();
        }

        private void SpawnRingParticles()
        {
            StartCoroutine(SpawnRingParticlesCoroutine(0.02f, 3));
            StartCoroutine(SpawnRingParticlesCoroutine(0.07f, 6));
            StartCoroutine(SpawnRingParticlesCoroutine(0.15f, 9));
        }

        private void ChangeColor()
        {
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
        
        #region Particles Methods
        
        private void SpawnHitParticles(GameObject objectHit)
        {
            var particlesObject = Instantiate(hitParticles, objectHit.transform.position, Quaternion.identity); // Provisional.

            var particlesController = particlesObject.GetComponent<MultipleParticlesController>();
            particlesController.Play(CurrentParticlesColor);

            var particlesFollower = particlesObject.GetComponent<ObjectFollower>();
            particlesFollower.SetTarget(objectHit.transform);
        }

        private void SpawnBounceParticles(Vector2 particlesPosition, Color particlesColor)
        {
            var bounceParticlesObject = Instantiate(bounceParticles, particlesPosition, Quaternion.identity); // Cambiar por Object Pool.
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
            var ringParticlesObject = (GameObject) Instantiate(ringParticles, transform.position, Quaternion.LookRotation(_rigidbody.velocity.normalized)); // Cambiar por Object Pool.
            var ringParticlesMain = ringParticlesObject.GetComponent<ParticleSystem>().main;
            ringParticlesMain.startSpeed = speed;
            ringParticlesMain.startColor = CurrentParticlesColor;
        }
        
        #endregion
        
        #endregion
    }
}
