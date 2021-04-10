using System;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Enemy;
using UnityEngine;

namespace OrbOfDeception.Player.Orb
{
    public sealed class OrbController : MonoBehaviour
    {
        #region Variables

        private enum OrbState
        {
            Idle,
            Returning,
            DirectionalAttack
        }

        private OrbState _state;

        [SerializeField] private InputManager inputManager;
        [SerializeField] private GameObject bounceParticles;
        [SerializeField] private Transform orbIdlePositionTransform;
        [SerializeField] private ParticleSystem directionAttackOrbParticles;
        [SerializeField] private ParticleSystem orbIdleParticles;
        [SerializeField] private ParticleSystem orbColorChangeParticles;
        [SerializeField] private Collider2D physicsCollider;
        [SerializeField] private float directionalAttackInitialForce;
        [SerializeField] private float attractionForce;
        [SerializeField] private float radiusToGoIdle;
        [SerializeField] private float idleFloatingMoveDistance = 1;
        [SerializeField] private float idleFloatingMoveVelocity = 1;
        [SerializeField] private float idleLerpPlayerFollowValue = 0.5f;
        [SerializeField] private float directionalAttackDecelerationFactor = 0.95f;
        [SerializeField] private float directionalAttackMinVelocityToChangeState = 1;
        [SerializeField] private float directionalAttackFirstBounceVelocityBoost = 1.5f;

        private bool _isWhite;
        private bool _hasReceivedAVelocityBoost;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        
        #endregion

        #region Methods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            physicsCollider.enabled = false;
            _state = OrbState.Idle;
            _isWhite = true;
            _hasReceivedAVelocityBoost = false;
            directionAttackOrbParticles.Stop();

            inputManager.DirectionalAttack = DirectionalAttack;
            inputManager.Click = OnMouseClick;
            inputManager.ChangeOrbColor = ChangeColor;
        }

        private void Update()
        {
            switch (_state)
            {
                case OrbState.Idle:
                    //transform.position = orbIdlePositionTransform.position;
                    /*if (UnityEngine.Input.GetMouseButtonDown(0))
                    {
                        physicsCollider.enabled = true;
                        _hasReceivedAVelocityBoost = false;
                        _state = OrbState.DirectionalAttack;
                        orbIdleParticles.Stop();
                        directionAttackOrbParticles.Play();
                        var worldPosition2D = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                        worldPosition2D.z = transform.position.z;
                        _directionalAttackDirection = (worldPosition2D - transform.position).normalized;
                        _rigidbody.AddForce(_directionalAttackDirection * directionalAttackInitialForce, ForceMode2D.Impulse);
                    }*/
                    break;
                case OrbState.DirectionalAttack:
                    if (_rigidbody.velocity.magnitude <= directionalAttackMinVelocityToChangeState)
                    {
                        physicsCollider.enabled = false;
                        _state = OrbState.Returning;
                    }
                    break;
                case OrbState.Returning:
                    if (Vector2.Distance(transform.position, orbIdlePositionTransform.position) <= radiusToGoIdle)
                    {
                        _rigidbody.velocity = Vector2.zero;
                        orbIdleParticles.Play();
                        directionAttackOrbParticles.Stop();
                        _state = OrbState.Idle;
                    }
                    break;
            }
        }

        private void OnMouseClick(Vector2 mousePosition)
        {
            var worldPosition2D = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            worldPosition2D.z = transform.position.z;
            var direction = (worldPosition2D - transform.position).normalized;
            
            DirectionalAttack(direction);
        }

        private void DirectionalAttack(Vector2 direction)
        {
            if (_state != OrbState.Idle)
            {
                return;
            }
            
            physicsCollider.enabled = true;
            _hasReceivedAVelocityBoost = false;
            _state = OrbState.DirectionalAttack;
            orbIdleParticles.Stop();
            directionAttackOrbParticles.Play();
            _rigidbody.AddForce(direction * directionalAttackInitialForce, ForceMode2D.Impulse);
        }
        
        private void FixedUpdate()
        {
            switch (_state)
            {
                case OrbState.Idle:
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
            _isWhite = !_isWhite;
            
            var directionalAttackParticles = directionAttackOrbParticles.main;
            var idleParticles = orbIdleParticles.main;
            var colorChangeParticles = orbColorChangeParticles.main;
            
            if (_isWhite)
            {
                _spriteRenderer.color = Color.white;
                directionalAttackParticles.startColor = Color.white;
                idleParticles.startColor = Color.white;
                colorChangeParticles.startColor = Color.white;
            }
            else
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy") || _state == OrbState.Idle) return;
            
            var enemy = other.GetComponent<EnemyController>();

            if ((!enemy.IsWhite && _isWhite) || (enemy.IsWhite && !_isWhite))
            {
                other.GetComponent<EnemyController>().ReceiveDamage(10);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
            
            SpawnBounceParticles(other.contacts[0].point, _isWhite ? Color.white : new Color(0.1f, 0.1f, 0.1f));
            
            if (_state != OrbState.DirectionalAttack || _hasReceivedAVelocityBoost)
                return;
            
            var newVelocity = _rigidbody.velocity;
            newVelocity += newVelocity.normalized * directionalAttackFirstBounceVelocityBoost;
            _rigidbody.velocity = newVelocity;
                
            _hasReceivedAVelocityBoost = true;
        }

        private void SpawnBounceParticles(Vector2 particlesPosition, Color particlesColor)
        {
            var bounceParticlesObject = Instantiate(bounceParticles, particlesPosition, Quaternion.identity); // Cambiar por Object Pool.
            var bounceParticlesMain = bounceParticlesObject.GetComponent<ParticleSystem>().main;

            var collisionScreenPosition = Camera.main.WorldToScreenPoint(particlesPosition);
            
            var t2d = 
            
            bounceParticlesMain.startColor = particlesColor;
        }
        
        #endregion
    }
}
