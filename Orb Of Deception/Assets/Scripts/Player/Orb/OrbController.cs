using System;
using Nanref.Enemy;
using UnityEngine;

namespace Nanref.Player.Orb
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
        [SerializeField] private float directionalAttackInitialVelocity;
        [SerializeField] private float attractionForce;
        [SerializeField] private float radiusToGoIdle;
        [SerializeField] private float idleFloatingMoveDistance = 1;
        [SerializeField] private float idleFloatingMoveVelocity = 1;
        [SerializeField] private float idleLerpPlayerFollowValue = 0.5f;

        private bool _isWhite;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _directionalAttackDirection;
        
        #endregion

        #region Methods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _state = OrbState.Idle;
            _isWhite = true;
        }

        private void Update()
        {
            switch (_state)
            {
                case OrbState.Idle:
                    //transform.position = orbIdlePositionTransform.position;
                    if (Input.GetMouseButtonDown(0))
                    {
                        _state = OrbState.DirectionalAttack;
                        _directionalAttackDirection =
                            ((Vector2) Input.mousePosition -
                             new Vector2((float) Screen.width / 2, (float) Screen.height / 2)).normalized;
                        _rigidbody.velocity = _directionalAttackDirection * directionalAttackInitialVelocity;
                    }
                    break;
                case OrbState.DirectionalAttack:
                    if (Vector3.Dot(_directionalAttackDirection, _rigidbody.velocity.normalized) < 0.9f)
                    {
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
                    
                    newPosition.x =  Mathf.Lerp(currentPosition.x, orbIdlePosition.x, idleLerpPlayerFollowValue);
                    newPosition.y = orbIdlePosition.y + Mathf.Sin(Time.time * idleFloatingMoveVelocity) *
                        idleFloatingMoveDistance;
                    
                    transform.position = newPosition;
                    
                    break;
                case OrbState.DirectionalAttack:
                    _rigidbody.AddForce(-_directionalAttackDirection * attractionForce);
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

        #endregion
    }
}
