using System;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class Boss1Controller : EnemyController
    {

        #region Variables

        [Header("Boss 1 References")]
        
        [Space]
        
        public ParticleSystem idleParticles;
        public LightController lightController;
        
        [Space]
        
        public MultipleParticlesController teleportToParticles;
        public ParticleSystem teleportFromParticles;
        public Transform[] teleportPoints;
        public Action onTeleport;
        
        [Space]
        
        public ParticleSystem chargeParticles;
        public GameObject chargeRingParticles;

        public Boss1SpellCaster spellCaster { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public Boss1Parameters Parameters => BaseParameters as Boss1Parameters;
        public Boss1MaskController MaskController { get; private set; }
        
        private static readonly int Hidden = Animator.StringToHash("Hidden");
        
        public const int AppearingState = 0;
        public const int TeleportingState = 1;
        public const int ChargeState = 2;
        public const int SpellState = 3;

        #endregion
        
        #region Methods
        
        protected override void OnAwake()
        {
            base.OnAwake();

            Rigidbody = GetComponent<Rigidbody2D>();
            spellCaster = GetComponentInChildren<Boss1SpellCaster>();
            MaskController = GetComponentInChildren<Boss1MaskController>();
            
            AddState(AppearingState, new AppearingState(this));
            AddState(TeleportingState, new TeleportingState(this));
            AddState(ChargeState, new ChargeState(this));
            AddState(SpellState, new SpellState(this));
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            spriteAnim.SetTrigger(Hidden);
        }

        public void Appear()
        {
            SetState(AppearingState);
            MaskController.StartMaskColorChange();
        }

        public void InitBattle()
        {
            SetState(TeleportingState);
        }

        protected override void Die()
        {
            base.Die();
            
            idleParticles.Stop();
            lightController.Hide();
            MaskController.StopChangingColor();
            Rigidbody.velocity = Vector2.zero;
        }

        private void OnTeleport()
        {
            onTeleport?.Invoke();
        }
        
        #endregion
    }
}