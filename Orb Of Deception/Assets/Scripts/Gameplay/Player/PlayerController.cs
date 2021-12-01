using System.Collections.Generic;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Enemy;
using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class PlayerController : StateMachineController
    {
        #region Variables

        [SerializeField] private Texture2D cursorSprite;
        [SerializeField] private float groundVelocity = 6;
        [SerializeField] private float airVelocity;
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private float jumpTime = 1;
        [SerializeField] private float maxFallVelocity = -5;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float timeInvulnerable = 2;
        [SerializeField] private GameObject spriteObject;
        [SerializeField] private Transform[] groundDetectors;
        [SerializeField] private float groundDetectionRayDistance;
        [SerializeField] private InputManager inputManager;
        public const int InitialHealth = 4; // Provisional.

        [HideInInspector] public bool isControlled = true;
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Animator _spriteAnimator;
        
        public JumpController JumpController { get; private set; }
        public GroundDetector GroundDetector { get; private set; }
        public HorizontalMovementController HorizontalMovementController { get; private set; }
        public AnimationController AnimationController { get; private set; }
        public PlayerHealthController PlayerHealthController { get; private set; }
        public HurtController HurtController { get; private set; }
        public EssenceOfPunishmentCounter EssenceOfPunishmentCounter { get; private set; }
        public CollectibleCounter CollectibleCounter { get; private set; }
        public PlayerInteraction PlayerInteraction { get; private set; }
        public KneelController KneelController { get; private set; }
        
        public const int InControlState = 0;
        public const int KneelState = 1;
        public const int DashState = 2;
        #endregion
    
        #region Methods
        protected override void OnAwake()
        {
            base.OnAwake();
            
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteAnimator = spriteObject.GetComponent<Animator>();
            
            GroundDetector = new GroundDetector(groundDetectors, groundDetectionRayDistance, coyoteTime);
            JumpController = new JumpController(_rigidbody, jumpForce, jumpTime, maxFallVelocity, GroundDetector);
            HorizontalMovementController = new HorizontalMovementController(_rigidbody, groundVelocity, airVelocity, inputManager);
            AnimationController = new AnimationController(_animator, _rigidbody, GroundDetector);
            PlayerHealthController = new PlayerHealthController(this, InitialHealth);
            HurtController = new HurtController(this, timeInvulnerable, _spriteAnimator);
            EssenceOfPunishmentCounter = new EssenceOfPunishmentCounter();
            CollectibleCounter = new CollectibleCounter();
            PlayerInteraction = new PlayerInteraction();
            KneelController = new KneelController();
            
            inputManager.Jump = JumpController.Jump;
            inputManager.StopJumping = JumpController.StopJumping;
            
            AddState(InControlState, new InControlState());
            AddState(KneelState, new KneelState());
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            SetInitialState(InControlState);
            PlayerInteraction.Start();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            GroundDetector.Update(Time.deltaTime);
            AnimationController.Update();
            HorizontalMovementController.Update();
        }
        
        public void Die() // Provisional.
        {
            Debug.Log("Player died.");
        }
        
        public void GetDamaged()
        {
            if (HurtController.IsInvulnerable()) return;

            GameManager.Camera.Shake(0.7f); // Provisional.
            PlayerHealthController.ReceiveDamage();
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
    
    public class InControlState : State
    {
        public InControlState()
        {
        }

        public override void Enter()
        {
            base.Enter();

            GameManager.Player.isControlled = true;
        }

        public override void Exit()
        {
            base.Exit();
            
            GameManager.Player.isControlled = false;
        }
    }
    
    public class KneelState : State
    {
        public KneelState()
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Enter Kneel");
        }
    }
}
