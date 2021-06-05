using System;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Core.Scenes;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float velocity = 5;
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private float jumpTime = 1;
        [SerializeField] private GameObject spriteObject;
        [SerializeField] private Transform[] groundDetectors;
        [SerializeField] private float groundDetectionRayDistance;
        [SerializeField] private InputManager inputManager;
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Animator _spriteAnimator;
        private float _direction;
        
        private PlayerJumpController _playerJumpController;
        private PlayerGroundDetector _playerGroundDetector;
        private PlayerHorizontalMovementController _playerHorizontalMovementController;
        private PlayerAnimationController _playerAnimationController;
        private PlayerHealthController _playerHealthController;
        
        private static readonly int Invulnerable = Animator.StringToHash("Invulnerable");

        public float Direction => _direction; // Cambiar.
        #endregion
    
        #region Methods
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteAnimator = spriteObject.GetComponent<Animator>();
            
            _playerGroundDetector = new PlayerGroundDetector(groundDetectors, groundDetectionRayDistance);
            _playerJumpController = new PlayerJumpController(_rigidbody, jumpForce, jumpTime, _playerGroundDetector);
            _playerHorizontalMovementController = new PlayerHorizontalMovementController(_rigidbody, velocity, inputManager);
            _playerAnimationController = new PlayerAnimationController(_animator, _rigidbody, inputManager, _playerGroundDetector);
            
            inputManager.Jump = _playerJumpController.Jump;
            inputManager.StopJumping = _playerJumpController.StopJumping;
        }

        private void Update()
        {
            _direction = inputManager.GetHorizontal();

            _playerGroundDetector.Update();
            
            // Provisional.
            if (_direction != 0)
            {
                var directionRaw = (_direction > 0) ? 1 : -1;
                spriteObject.transform.localScale = new Vector3(directionRaw, 1, 1);
            }
            // Fin provisional.
            
            _playerAnimationController.Update();
        }

        public void GetDamaged(int damage)
        {
            _playerHealthController.ReceiveDamage(damage);
            //_playerAnimationController.PlayHurtAnimation();
        }
        
        private void OnDrawGizmos()
        {
            _playerGroundDetector?.OnDrawGizmos();
        }

        private void FixedUpdate()
        {
            _playerHorizontalMovementController?.FixedUpdate();
            _playerJumpController?.FixedUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Enemigo colisionado");
                _spriteAnimator.SetTrigger(Invulnerable);
            }
        }

        #endregion
    }
}
