using System;
using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public abstract class EnemyController : StateMachineController
    {
        #region Variables

        [Header("Shared Enemy variables")]
        [SerializeField] protected Animator spriteAnim;
        [SerializeField] private EnemyDamagingArea damagingArea;
        
        private float _health;
        private bool _hasBeenSpawned;
        
        protected EnemyParameters parameters;
        private EssenceOfPunishmentSpawner _essenceOfPunishmentSpawner;
        private EnemyDeathParticles _enemyDeathParticles;
        
        private static readonly int BeingHurt = Animator.StringToHash("Hurt");
        private static readonly int Dying = Animator.StringToHash("Die");

        public Action onDie;
        public Action onMaskColorChange;
        
        #endregion
        
        #region Properties
        
        public Animator Anim  { private set; get; }
        
        #endregion

        #region Methods
        
        #region MonoBehaviour Methods
        protected override void OnAwake()
        {
            base.OnAwake();
            
            Anim = GetComponent<Animator>();
            parameters = GetComponent<EnemyParameters>();
            parameters.enemyController = this;
            _health = parameters.Stats.health;
            _essenceOfPunishmentSpawner = GetComponentInChildren<EssenceOfPunishmentSpawner>();
            _enemyDeathParticles = GetComponentInChildren<EnemyDeathParticles>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            if (!_hasBeenSpawned)
            {
                SetOrientation(parameters.orientationIsRight);
            }
        }

        #endregion
        
        #region Shared Enemy Methods
        protected virtual void Die()
        {
            stateMachine.ExitState();
            Anim.enabled = false;
            spriteAnim!.SetTrigger(Dying);
            
            var damageableAreas = GetComponentsInChildren<EnemyDamageableArea>();
            foreach (var damageableArea in damageableAreas)
            {
                damageableArea.DisableCollider();
            }

            damagingArea.DisableCollider();
            
            HideShadows();
            
            _enemyDeathParticles.PlayParticles();
            _essenceOfPunishmentSpawner.SpawnEssences(parameters.Stats.essenceOfPunishmentAmount);
            
            onDie?.Invoke();
        }

        private void HideShadows()
        {
            var shadows = GetComponentsInChildren<GroundShadowController>();
            foreach (var shadow in shadows)
            {
                shadow.Hide();
            }
        }
        
        private void AppearShadows()
        {
            var shadows = GetComponentsInChildren<GroundShadowController>();
            foreach (var shadow in shadows)
            {
                shadow.Appear();
            }
        }
        
        public void GetDamaged(GameEntity.EntityColor damageColor, int damage)
        {
            if (parameters.maskColor != damageColor)
            {
                ReceiveDamage(damage);
            }
        }

        protected virtual void ReceiveDamage(float damage)
        {
            if (_health <= 0)
                return;
            
            _health = Mathf.Max(0, _health - damage);

            GameManager.Orb.SpawnHitParticles(gameObject);
            
            if (_health <= 0)
                Die();
            else
                Hurt();
        }

        protected virtual void Hurt()
        {
            spriteAnim!.SetTrigger(BeingHurt);
        }
        
        public GameEntity.EntityColor GetMaskColor()
        {
            return parameters.maskColor;
        }

        public void OnSpawn(GameEntity.EntityColor newColor, bool isOrientationRight)
        {
            _hasBeenSpawned = true;
            SetOrientation(isOrientationRight);
            parameters.maskColor = newColor;
            onMaskColorChange?.Invoke();
            AppearShadows();
            spriteAnim.SetTrigger("Appear"); // Encapsular en un script aparte.
        }

        public virtual void SetOrientation(bool isOrientationRight)
        {
            
        }
        
        #endregion
        
        #endregion
    }
}

