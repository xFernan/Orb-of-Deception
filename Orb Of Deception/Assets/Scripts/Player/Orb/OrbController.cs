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
        
        [SerializeField] private Transform orbIdlePositionTransform;
        [SerializeField] private ParticleSystem orbParticles;
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
                    var newPosition = currentPosition;
                    var orbIdlePosition = orbIdlePositionTransform.position;

                    newPosition = Vector2.Lerp(currentPosition, orbIdlePosition, idleLerpPlayerFollowValue);
                    
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
            _spriteRenderer.color = _isWhite ? Color.white : new Color(0.1f, 0.1f, 0.1f);
            var particles = orbParticles.main;
            particles.startColor = _isWhite ? Color.white : Color.black;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radiusToGoIdle);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;
            
            var enemy = other.GetComponent<EnemyController>();

            if ((!enemy.IsWhite && _isWhite) || (enemy.IsWhite && !_isWhite))
            {
                other.GetComponent<EnemyController>().ReceiveDamage(10);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_state != OrbState.DirectionalAttack || _hasReceivedAVelocityBoost ||
                other.gameObject.layer != LayerMask.NameToLayer("Ground"))
                return;
            
            var newVelocity = _rigidbody.velocity;
            newVelocity += newVelocity.normalized * directionalAttackFirstBounceVelocityBoost;
            _rigidbody.velocity = newVelocity;
                
            _hasReceivedAVelocityBoost = true;
        }

        #endregion
    }
}
