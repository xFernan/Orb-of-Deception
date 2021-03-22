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
                    transform.position = orbIdlePositionTransform.position;
                    if (Input.GetMouseButtonDown(0))
                    {
                        _state = OrbState.DirectionalAttack;
                        directionalAttackDirection = ((Vector2) Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2)).normalized;
                        _rigidbody.velocity = directionalAttackDirection * directionalAttackInitialVelocity;
                    }
                    break;
                case OrbState.DirectionalAttack:
                    if (_rigidbody.velocity.magnitude <= 0.3f)
                    {
                        _state = OrbState.Returning;
                    }
                    else
                    {
                        _rigidbody.AddForce(-directionalAttackDirection * attractionForce);
                    }
                    break;
                case OrbState.Returning:
                    Debug.Log(_rigidbody.velocity.magnitude);
                    if (Vector2.Distance(transform.position, orbIdlePositionTransform.position) <= radiusToGoIdle)
                    {
                        _rigidbody.velocity = Vector2.zero;
                        _state = OrbState.Idle;
                    }
                    else
                    {
                        var velocityValue = _rigidbody.velocity.magnitude;
                        var direction = (orbIdlePositionTransform.position - transform.position).normalized;
                        _rigidbody.velocity = direction * velocityValue;
                        _rigidbody.AddForce(direction * attractionForce);
                    }
                    break;
            }

            if (Input.GetMouseButtonDown(1))
            {
                ChangeColor();
            }
        }

        private void ChangeColor()
        {
            _isWhite = !_isWhite;
            if (_isWhite)
            {
                _spriteRenderer.color = Color.white;
            }
            else
            {
                _spriteRenderer.color = Color.gray;
            }
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
