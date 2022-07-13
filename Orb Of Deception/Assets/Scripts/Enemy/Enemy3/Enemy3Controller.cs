using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    public class Enemy3Controller : EnemyController
    {
        #region Variables
        
        public Enemy3Parameters Parameters => BaseParameters as Enemy3Parameters;
        
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public SurroundingsDetector SurroundingsDetector { get; private set; }
        public Enemy3StaffController StaffController { get; private set; }
        
        public const int IdleState = 0;
        public const int AttackingState = 1;

        #endregion
        
        #region Methods
        
        protected override void OnAwake()
        {
            base.OnAwake();

            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = spriteAnim.gameObject.GetComponent<SpriteRenderer>();
            StaffController = GetComponentInChildren<Enemy3StaffController>();
            SurroundingsDetector = GetComponentInChildren<SurroundingsDetector>();
            
            AddState(IdleState,
                new Enemy3WalkingState(this));
            AddState(AttackingState,
                new Enemy3AttackState(this));
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            if (BaseParameters.hasBeenSpawned)
            {
                SetInitialState(AttackingState);
            }
            else
            {
                SetInitialState(IdleState);
            }
        }
        
        private void OnDrawGizmos()
        {
            if (BaseParameters == null) return;
            
            var position = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, Parameters.distanceToAttack);
        }

        public override void SetOrientation(bool isOrientationRight)
        {
            SpriteRenderer.flipX = !isOrientationRight; // Provisional, hacer con animaciones para cambiar sombras.
            StaffController.UpdateXOffset(isOrientationRight);
        }
        
        private void PlayStepSound()
        {
            soundsPlayer.Play("Step");
        }
        
        #endregion
    }
}
