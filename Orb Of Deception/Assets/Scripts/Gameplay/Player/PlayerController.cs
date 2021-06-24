using System;
using DG.Tweening;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core.Input;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float velocity = 5;
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private float jumpTime = 1;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float timeInvulnerable = 2;
        [SerializeField] private GameObject spriteObject;
        [SerializeField] private Transform[] groundDetectors;
        [SerializeField] private float groundDetectionRayDistance;
        [SerializeField] private InputManager inputManager;
        private int initialHealth = 100; // Provisional.
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Animator _spriteAnimator;
        private float _direction;
        
        public JumpController JumpController { get; private set; }
        public GroundDetector GroundDetector { get; private set; }
        public HorizontalMovementController HorizontalMovementController { get; private set; }
        public AnimationController AnimationController { get; private set; }
        public HealthController HealthController { get; private set; }
        public HurtController HurtController { get; private set; }

        public static Action<int> onDirectionChanged;
        public float Direction { get; private set; }
        public bool IsMoving { get; private set; }
        #endregion
    
        #region Methods
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteAnimator = spriteObject.GetComponent<Animator>();
            
            GroundDetector = new GroundDetector(groundDetectors, groundDetectionRayDistance, coyoteTime);
            JumpController = new JumpController(_rigidbody, jumpForce, jumpTime, GroundDetector);
            HorizontalMovementController = new HorizontalMovementController(_rigidbody, velocity, inputManager);
            AnimationController = new AnimationController(_animator, _rigidbody, inputManager, GroundDetector);
            HealthController = new HealthController(this, initialHealth);
            HurtController = new HurtController(timeInvulnerable, _spriteAnimator);
            
            inputManager.Jump = JumpController.Jump;
            inputManager.StopJumping = JumpController.StopJumping;
        }

        private void Update()
        {
            var moveDirection = inputManager.GetHorizontal();

            GroundDetector.Update(Time.deltaTime);
            
            HurtController.Update(Time.deltaTime);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                GetDamaged(10);
            }

            IsMoving = moveDirection != 0;
            
            if (IsMoving)
            {
                var haveDirectionChanged = Direction != moveDirection;

                Direction = moveDirection;
                
                if (haveDirectionChanged)
                    onDirectionChanged((int) moveDirection);
            }
            
            AnimationController.Update();
        }

        public void Die() // Provisional.
        {
            Debug.Log("Player died.");
        }
        
        public void GetDamaged(int damage)
        {
            if (HurtController.IsInvulnerable()) return;

            Camera.main.GetComponentInParent<CameraController>().Shake(0.7f); // Provisional.
            HealthController.ReceiveDamage(damage);
            HurtController.StartHurt();
        }
        
        private void OnDrawGizmos()
        {
            GroundDetector?.OnDrawGizmos();
        }

        private void FixedUpdate()
        {
            HorizontalMovementController?.FixedUpdate();
            JumpController?.FixedUpdate();
        }
        #endregion
    }
}
