using System;
using System.Collections;
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
        public SoundsPlayer soundsPlayer;
        public Animator spriteAnim;
        [SerializeField] private EnemyDamagingArea damagingArea;
        
        private float _health;
        private bool _hasDied = false;

        public EnemyParameters BaseParameters { get; private set; }
        private EssenceOfPunishmentSpawner _essenceOfPunishmentSpawner;
        private EnemyDeathParticles _enemyDeathParticles;
        private SpriteRenderer[] _spriteRenderers;
        
        private static readonly int BeingHurt = Animator.StringToHash("Hurt");
        private static readonly int Dying = Animator.StringToHash("Die");

        public Action onDie;
        
        #endregion
        
        #region Properties
        
        public Animator Animator  { private set; get; }
        
        #endregion

        #region Methods
        
        #region MonoBehaviour Methods
        protected override void OnAwake()
        {
            base.OnAwake();
            
            Animator = GetComponent<Animator>();
            BaseParameters = GetComponent<EnemyParameters>();
            _health = BaseParameters.Stats.health;
            _essenceOfPunishmentSpawner = GetComponentInChildren<EssenceOfPunishmentSpawner>();
            _enemyDeathParticles = GetComponentInChildren<EnemyDeathParticles>();
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
            if (_hasDied)
                return;

            _hasDied = true;
            
            ExitState();
            if (Animator != false)
                Animator.enabled = false;
            spriteAnim!.SetTrigger(Dying);
            
            var damageableAreas = GetComponentsInChildren<EnemyDamageableArea>();
            foreach (var damageableArea in damageableAreas)
            {
                damageableArea.DisableCollider();
            }

            damagingArea.DisableCollider();
            
            HideShadows();
            
            _enemyDeathParticles.PlayParticles();
            
            if (BaseParameters.doesDropEssences)
                _essenceOfPunishmentSpawner.SpawnEssences(BaseParameters.Stats.essenceOfPunishmentAmount);
            
            soundsPlayer.Play("Dying");
            
            onDie?.Invoke();

            if (!BaseParameters.hasBeenSpawned)
            {
                SaveSystem.AddEnemyDead(BaseParameters.id);
            }
        }

        public void Kill()
        {
            Die();
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
            if (BaseParameters.MaskColor != damageColor)
            {
                ReceiveDamage(damage);
                GameManager.Orb.SoundsPlayer.Play("Damaging");
            }
            else
            {
                GameManager.Orb.SoundsPlayer.Play("NotDamaging");
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
            return BaseParameters.MaskColor;
        }

        public virtual void OnSpawn(GameEntity.EntityColor newColor, bool isOrientationRight)
        {
            BaseParameters.hasBeenSpawned = true;
            SetOrientation(isOrientationRight);
            BaseParameters.MaskColor = newColor;
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

