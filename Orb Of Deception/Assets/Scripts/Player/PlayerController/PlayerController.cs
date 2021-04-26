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
        private float _direction;
        
        private PlayerJumpController _playerJumpController;
        private PlayerGroundDetector _playerGroundDetector;
        private PlayerHorizontalMovementController _playerHorizontalMovementController;
        
        private PlayerAnimationController _playerAnimationController;

        public float Direction => _direction; // Cambiar.
        #endregion
    
        #region Methods
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
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

        private void OnDrawGizmos()
        {
            _playerGroundDetector?.OnDrawGizmos();
        }

        private void FixedUpdate()
        {
            _playerHorizontalMovementController?.FixedUpdate();
            _playerJumpController?.FixedUpdate();
        }
        #endregion
    }
}
