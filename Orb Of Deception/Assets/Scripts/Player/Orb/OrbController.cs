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
        [SerializeField] private float directionalAttackInitialVelocity;
        [SerializeField] private float attractionForce;
        [SerializeField] private float radiusToGoIdle;
        [SerializeField] private float idleYShiftValue = 1;

        private bool _isWhite;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Vector2 directionalAttackDirection;
        
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
                        directionalAttackDirection =
                            ((Vector2) Input.mousePosition -
                             new Vector2((float) Screen.width / 2, (float) Screen.height / 2)).normalized;
                        _rigidbody.velocity = directionalAttackDirection * directionalAttackInitialVelocity;
                    }
                    break;
                case OrbState.DirectionalAttack:
                    if (Vector3.Dot(directionalAttackDirection, _rigidbody.velocity.normalized) < 0.9f)
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
                    
                    newPosition.x =  Mathf.Lerp(currentPosition.x, orbIdlePosition.x, 0.7f);
                    newPosition.y = orbIdlePosition.y + Mathf.Sin(Time.time) * idleYShiftValue;
                    
                    transform.position = newPosition;
                    
                    break;
                case OrbState.DirectionalAttack:
                    _rigidbody.AddForce(-directionalAttackDirection * attractionForce);
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
            _spriteRenderer.color = _isWhite ? Color.white : Color.gray;
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
