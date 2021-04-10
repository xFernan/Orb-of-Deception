using System.Linq;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OrbOfDeception.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float velocity = 5;
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private GameObject spriteObject;
        [SerializeField] private Transform[] groundDetectors;
        [SerializeField] private float groundDetectionRayDistance;
        [SerializeField] private InputManager inputManager;
        
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
            
            inputManager.Jump = Jump;
        }

        private void Update()
        {
            // Detección del input.
            _direction = inputManager.GetHorizontal();

            // Hacer script aparte.
            if (_direction != 0)
            {
                var directionRaw = (_direction > 0) ? 1 : -1;
                spriteObject.transform.localScale = new Vector3(directionRaw, 1, 1);
            }
            
            _animator.SetBool("IsMoving", IsOnTheGround() && _direction != 0);
        }

        private void Jump()
        {
            Debug.Log("Saltando");
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
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
