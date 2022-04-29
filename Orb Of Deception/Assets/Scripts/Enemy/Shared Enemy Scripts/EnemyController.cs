using System;
using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using OrbOfDeception.Essence_of_Punishment;
using OrbOfDeception.Player;
using OrbOfDeception.Rooms;
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

        protected EnemyParameters BaseParameters { get; private set; }
        public SoundsPlayer SoundsPlayer { get; private set; }
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
            BaseParameters = GetComponent<EnemyParameters>();
            SoundsPlayer = GetComponentInChildren<SoundsPlayer>();
            _health = BaseParameters.Stats.health;
            _essenceOfPunishmentSpawner = GetComponentInChildren<EssenceOfPunishmentSpawner>();
            _enemyDeathParticles = GetComponentInChildren<EnemyDeathParticles>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (!BaseParameters.hasBeenSpawned)
            {
                if (SaveSystem.IsEnemyDead(BaseParameters.id))
                {
                    gameObject.SetActive(false);
                    return;
                }
                
                SetOrientation(BaseParameters.orientationIsRight);
            }
        }

        #endregion
        
        #region Shared Enemy Methods
        protected virtual void Die()
        {
            stateMachine.ExitState();
            if (Anim != false)
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
            _essenceOfPunishmentSpawner.SpawnEssences(BaseParameters.Stats.essenceOfPunishmentAmount);
            
            SoundsPlayer.Play("Dying");
            
            onDie?.Invoke();

            if (!BaseParameters.hasBeenSpawned)
            {
                SaveSystem.AddEnemyDead(BaseParameters.id);
            }
        }

        private void HideShadows()
        {
            var shadows = GetComponentsInChildren<GroundShadowController>();
            foreach (var shadow in shadows)
            {
                shadow.HideWithDelay();
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
            if (BaseParameters.maskColor != damageColor)
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
            return BaseParameters.maskColor;
        }

        public virtual void OnSpawn(GameEntity.EntityColor newColor, bool isOrientationRight)
        {
            BaseParameters.hasBeenSpawned = true;
            SetOrientation(isOrientationRight);
            BaseParameters.maskColor = newColor;
            onMaskColorChange?.Invoke();
            AppearShadows();
            spriteAnim.SetTrigger("Appear"); // Encapsular en un script aparte.
        }
        
        public bool IsSeeingPlayer(float distance)
        {
            var position = transform.position;
            var playerPosition = GameManager.Player.transform.position;
            var raycastDirection = (playerPosition - position).normalized;
            var distanceToPlayer = (playerPosition - position).magnitude;
            
            var raycastHit =
                Physics2D.Raycast(position, raycastDirection,
                    Mathf.Min(distanceToPlayer, distance), LayerMask.GetMask("Ground"));
            var isRaycastingObstacle = raycastHit.collider != null;
            
            var distanceFromPlayer = Vector2.Distance(playerPosition, position);

            return distanceFromPlayer <= distance && !isRaycastingObstacle;
        }

        public virtual void SetOrientation(bool isOrientationRight)
        {
            
        }
        
        #endregion
        
        #endregion
    }
}

