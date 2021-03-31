using System;
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

        [SerializeField] private GameObject bounceParticles;
        [SerializeField] private Transform orbIdlePositionTransform;
        [SerializeField] private ParticleSystem directionAttackOrbParticles;
        [SerializeField] private ParticleSystem idleOrbParticles;
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
        private Vector2 _directionalAttackDirection;
        
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
        }

        private void Update()
        {
            switch (_state)
            {
                case OrbState.Idle:
                    //transform.position = orbIdlePositionTransform.position;
                    if (Input.GetMouseButtonDown(0))
                    {
                        physicsCollider.enabled = true;
                        _hasReceivedAVelocityBoost = false;
                        _state = OrbState.DirectionalAttack;
                        idleOrbParticles.Stop();
                        directionAttackOrbParticles.Play();
                        _directionalAttackDirection =
                            ((Vector2) Input.mousePosition -
                             new Vector2((float) Screen.width / 2, (float) Screen.height / 2)).normalized;
                        _rigidbody.AddForce(_directionalAttackDirection * directionalAttackInitialForce, ForceMode2D.Impulse);
                    }
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
                        idleOrbParticles.Play();
                        directionAttackOrbParticles.Stop();
                        _state = OrbState.Idle;
                    }
                    break;
            }

            if (Input.GetMouseButtonDown(1))
            {
                ChangeColor();
            }
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
            var idleParticles = idleOrbParticles.main;
            
            if (_isWhite)
            {
                _spriteRenderer.color = Color.white;
                directionalAttackParticles.startColor = Color.white;
                idleParticles.startColor = Color.white;
            }
            else
            {
                _spriteRenderer.color = new Color(0.1f, 0.1f, 0.1f);
                directionalAttackParticles.startColor = Color.black;
                idleParticles.startColor = Color.black;
            }
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
            
            var bounceParticlesObject = Instantiate(bounceParticles, other.contacts[0].point, Quaternion.identity);
            var bounceParticlesMain = bounceParticlesObject.GetComponent<ParticleSystem>().main;
            bounceParticlesMain.startColor = _isWhite ? Color.white : new Color(0.1f, 0.1f, 0.1f);
            
            if (_state != OrbState.DirectionalAttack || _hasReceivedAVelocityBoost)
                return;
            
            var newVelocity = _rigidbody.velocity;
            newVelocity += newVelocity.normalized * directionalAttackFirstBounceVelocityBoost;
            _rigidbody.velocity = newVelocity;
                
            _hasReceivedAVelocityBoost = true;
        }

        #endregion
    }
}
