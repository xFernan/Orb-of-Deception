using System.Collections.Generic;
using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public abstract class EnemyController : GameEntity
    {
        #region Variables

        [Header("Shared Enemy variables")]
        [SerializeField] protected Animator spriteAnim;
        [SerializeField] private EnemyDamagingArea damagingArea;
        
        private float _health;
        
        protected EnemyParameters parameters;
        private EssenceOfPunishmentSpawner _essenceOfPunishmentSpawner;
        private EnemyDeathParticles _enemyDeathParticles;
        
        private FiniteStateMachine _stateMachine;
        private Dictionary<int, State> _states;
        
        private static readonly int BeingHurt = Animator.StringToHash("Hurt");
        private static readonly int Dying = Animator.StringToHash("Die");

        #endregion
        
        #region Properties
        
        public Animator Anim  { private set; get; }
        
        #endregion

        #region Methods
        
        #region MonoBehaviour Methods
        private void Awake()
        {
            _stateMachine = new FiniteStateMachine();
            _states = new Dictionary<int, State>();
            
            Anim = GetComponent<Animator>();
            parameters = GetComponent<EnemyParameters>();
            _health = parameters.Stats.health;
            _essenceOfPunishmentSpawner = GetComponentInChildren<EssenceOfPunishmentSpawner>();
            _enemyDeathParticles = GetComponentInChildren<EnemyDeathParticles>();
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {
            
        }
        
        private void Update()
        {
            _stateMachine?.Update(Time.deltaTime);
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            
        }

        private void FixedUpdate()
        {
            _stateMachine?.FixedUpdate(Time.deltaTime);
            OnFixedUpdate();
        }

        protected virtual void OnFixedUpdate()
        {
            
        }

        #endregion
        
        #region State Machine Methods
        
        public void SetState(int stateId)
        {
            _stateMachine.SetState(_states[stateId]);
        }

        protected void SetInitialState(int stateId)
        {
            _stateMachine.SetInitialState(_states[stateId]);
        }
        
        protected void AddState(int stateId, State stateAdded)
        {
            _states.Add(stateId, stateAdded);
        }
        
        #endregion
        
        #region Shared Enemy Methods
        protected virtual void Die()
        {
            _stateMachine.ExitState();
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
        }

        private void HideShadows()
        {
            var shadows = GetComponentsInChildren<GroundShadowController>();
            foreach (var shadow in shadows)
            {
                shadow.Hide();
            }
        }
        
        public void GetDamaged(EntityColor damageColor, int damage)
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

            if (_health <= 0)
                Die();
            else
                Hurt();
        }

        protected virtual void Hurt()
        {
            spriteAnim!.SetTrigger(BeingHurt);
        }
        
        public EntityColor GetMaskColor()
        {
            return parameters.maskColor;
        }
        
        #endregion
        
        #endregion
    }
}

