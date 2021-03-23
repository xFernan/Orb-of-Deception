using System;
using System.Linq;
using Nanref.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Nanref.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float velocity = 5;
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private GameObject spriteObject;
        [SerializeField] private Transform[] groundDetectors;
        [SerializeField] private float groundDetectionRayDistance;
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private float _direction;

        public float Direction => _direction;
        #endregion
    
        #region Methods
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // Detección del input.
            _direction = Mathf.Clamp(Input.GetAxisRaw("Horizontal"), -1, 1);

            // Salto.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
            }

            // Para testear, se ha hecho que cada vez que se presiona la tecla Q, se dañará a un enemigo aleatorio.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var enemies = GameObject.FindObjectsOfType<EnemyController>();
                if (enemies.Length != 0)
                {
                    var enemyId = Random.Range(0, enemies.Length);
                    enemies[enemyId].ReceiveDamage(10);
                }
            }
        
            // Además, cada vez que se presiona la tecla E, se dañará a lenemigo más cercano.
            if (Input.GetKeyDown(KeyCode.E))
            {
                var enemies = GameObject.FindObjectsOfType<EnemyController>();
                if (enemies.Length != 0)
                {
                    var closestEnemy = enemies.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
                        .FirstOrDefault();
                    closestEnemy.ReceiveDamage(10);
                }
            }

            if (_direction != 0)
            {
                var directionRaw = (_direction > 0) ? 1 : -1;
                spriteObject.transform.localScale = new Vector3(directionRaw, 1, 1);
            }
            
            _animator.SetBool("IsMoving", IsOnTheGround() && _direction != 0);
        }

        private bool IsOnTheGround()
        {
            var isOnTheGround = false;
            foreach (var groundDetector in groundDetectors)
            {
                isOnTheGround |= Physics2D.Raycast(groundDetector.position, Vector2.down, groundDetectionRayDistance, LayerMask.GetMask("Ground"));
                if (isOnTheGround)
                    break;
            }

            return isOnTheGround;
        }
        
        private void OnDrawGizmos()
        {
            if (groundDetectors.Length == 0)
                return;

            foreach (var groundDetector in groundDetectors)
            {
                var position = groundDetector.position;
                Gizmos.DrawLine(position, position + Vector3.down * groundDetectionRayDistance);
            }
        }

        private void FixedUpdate()
        {
            //Movimiento según input.
            _rigidbody.velocity = new Vector2(velocity * _direction, _rigidbody.velocity.y);
        }

        #endregion
    }
}
