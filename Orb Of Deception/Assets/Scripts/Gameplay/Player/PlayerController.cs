using System.Collections;
using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Enemy;
using OrbOfDeception.Orb;
using OrbOfDeception.Rooms;
using OrbOfDeception.Statue;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerController : StateMachineController
    {
        #region Variables

        public SpriteRenderer bodySpriteRenderer;
        public SpriteRenderer maskSpriteRenderer;
        [SerializeField] private float groundVelocity = 6;
        [SerializeField] private float airVelocity;
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private float jumpTime = 1;
        [SerializeField] private float maxFallVelocity = -5;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float timeInvulnerable = 2;
        public GameObject spriteObject;
        [SerializeField] private Transform[] groundDetectors;
        [SerializeField] private Transform centralGroundDetector;
        [SerializeField] private float groundDetectionRayDistance;
        [SerializeField] private InputManager inputManager;
        public MultipleParticlesController deathParticlesController;
        public MultipleParticlesController jumpParticles;
        public ParticleSystem maskParticles;
        public const int InitialHealth = 5; // Provisional.

        public bool isControlled;
        private Coroutine _standCoroutine; // Refactorizable.
        
        public Rigidbody2D Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public Animator SpriteAnimator { get; private set; }
        public SoundsPlayer SoundsPlayer { get; private set; }
        
        public PlayerHorizontalMovementController HorizontalMovementController { get; private set; }
        public PlayerJumpController JumpController { get; private set; }
        public PlayerAnimationController AnimationController { get; private set; }
        public PlayerHealthController HealthController { get; private set; }
        public PlayerHurtController HurtController { get; private set; }
        public PlayerInteraction Interaction { get; private set; }
        public PlayerTriggerDetector TriggerDetector { get; private set; }
        public PlayerKneelController KneelController { get; private set; }
        public PlayerDeathController DeathController { get; private set; }
        public PlayerMaskController MaskController { get; private set; }
        public PlayerSpriteDirectionController SpriteDirectionController { get; private set; }
        public PlayerStatueMenuController StatueMenuController { get; private set; }
        
        public GroundDetector GroundDetector { get; private set; }
        public GroundShadowController GroundShadowController { get; private set; }
        public EssenceOfPunishmentCounter EssenceOfPunishmentCounter { get; private set; }
        
        private static readonly int Kneeled = Animator.StringToHash("Kneeled");
        
        public const int InControlState = 0;
        public const int KneelState = 1;
        public const int DashState = 2;
        
        #endregion
    
        #region Methods
        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            SpriteAnimator = spriteObject.GetComponent<Animator>();
            SoundsPlayer = GetComponentInChildren<SoundsPlayer>();
            
            GroundShadowController = GetComponentInChildren<GroundShadowController>();
            StatueMenuController = GetComponentInChildren<PlayerStatueMenuController>();
            MaskController = GetComponentInChildren<PlayerMaskController>();
            SpriteDirectionController = GetComponentInChildren<PlayerSpriteDirectionController>();
            
            GroundDetector = new GroundDetector(groundDetectors, centralGroundDetector, groundDetectionRayDistance, coyoteTime);
            JumpController = new PlayerJumpController(Rigidbody, jumpForce, jumpTime, maxFallVelocity, GroundDetector);
            HorizontalMovementController = new PlayerHorizontalMovementController(Rigidbody, groundVelocity, airVelocity, inputManager);
            AnimationController = new PlayerAnimationController(Animator, Rigidbody, GroundDetector);
            HealthController = new PlayerHealthController(this, InitialHealth);
            HurtController = new PlayerHurtController(this, timeInvulnerable, SpriteAnimator);
            EssenceOfPunishmentCounter = new EssenceOfPunishmentCounter();
            Interaction = new PlayerInteraction();
            TriggerDetector = new PlayerTriggerDetector();
            KneelController = new PlayerKneelController();
            DeathController = new PlayerDeathController();
            
            inputManager.Jump = JumpController.Jump;
            inputManager.StopJumping = JumpController.StopJumping;
            
            SaveSystem.UnlockMask(PlayerMaskController.MaskType.ShinyMask);
            
            AddState(InControlState, new NormalState());
            AddState(KneelState, new KneelState());
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            SetInitialState(InControlState);
            Interaction.Start();
            
            // PROVISIONAL
            SaveSystem.InitTimeCounter();
        }

        public void SpawnStand()
        {
            if (_standCoroutine != null) StopCoroutine(_standCoroutine);
            _standCoroutine = StartCoroutine(SpawnStandCoroutine());
        }
        
        private IEnumerator SpawnStandCoroutine()
        {
            HorizontalMovementController.SetDirection(1); // Provisional, es muy cutre.
            KneelController.Kneel();
            GameManager.Orb.SetHidden();
            yield return 0;
            Animator.Play("Kneeling", 0, 1); // Provisional.
            yield return new WaitForSeconds(1.5f);
            KneelController.Stand();
            yield return new WaitForSeconds(1);
            isControlled = true;
            if (SaveSystem.currentOrbType != OrbController.OrbType.None)
                GameManager.Orb.Appear();
        }
        
        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            GroundDetector.Update(Time.deltaTime);
            AnimationController.Update();
            HorizontalMovementController.Update();
        }
        
        public void GetDamaged()
        {
            if (HurtController.IsInvulnerable() || DeathController.isDying) return;

            GameManager.Camera.Shake(0.7f); // Provisional.
            HealthController.ReceiveDamage();
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
