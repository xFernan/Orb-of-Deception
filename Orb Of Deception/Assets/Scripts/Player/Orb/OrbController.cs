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

        private Rigidbody2D _rigidbody;
        private Vector2 directionalAttackDirection;
        
        #endregion

        #region Methods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _state = OrbState.Idle;
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
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radiusToGoIdle);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyController>().ReceiveDamage(10);
            }
        }

        #endregion
    }
}
