using System;
using System.Collections.Generic;
using OrbOfDeception.Patterns;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public abstract class EnemyController : GameEntity
    {
        
        #region Variables

        [Header("Shared Enemy variables")]
        [SerializeField] protected Animator spriteAnim;
        [SerializeField] protected EntityColor maskColor;
        [SerializeField] protected float health;
        
        private FiniteStateMachine _stateMachine;
        private Dictionary<int, EnemyState> _states;
        
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Dying = Animator.StringToHash("Die");

        #endregion
        
        #region Properties
        
        public Action GoToNextStateCallback { set; private get; }
        public Animator Anim  { private set; get; }
        public bool IsWhite => maskColor == EntityColor.White;
        private bool _isDying = false;
        
        #endregion

        #region Methods
        
        #region MonoBehaviour Methods
        protected virtual void Awake()
        {
            _stateMachine = new FiniteStateMachine();
            Anim = GetComponent<Animator>();
            _states = new Dictionary<int, EnemyState>();
        }

        protected virtual void Update()
        {
            _stateMachine?.Update(Time.deltaTime);
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine?.FixedUpdate(Time.deltaTime);
        }
        
        // Provisional
        private void OnTriggerStay2D(Collider2D other)
        {
            if (_isDying) return;
            
            var playerAreaDamage = other.GetComponent<PlayerAreaDamage>();
            if (playerAreaDamage != null)
            {
                playerAreaDamage.ReceiveDamage(10);
            }
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
        
        protected void AddState(int stateId, EnemyState stateAdded)
        {
            _states.Add(stateId, stateAdded);
        }
        
        private void GoToNextState()
        {
            GoToNextStateCallback?.Invoke();
        }

        private void OnDieAnimationEnded()
        {
            Destroy(gameObject);
        }
        
        #endregion
        
        #region Shared Enemy Methods
        protected virtual void Die()
        {
            _isDying = true;
            _stateMachine.ExitState();
            Anim.enabled = false;
            spriteAnim!.SetTrigger(Dying);
        }

        public void GetDamaged(EntityColor damageColor, int damage)
        {
            Debug.Log("Get damaged");
            if (maskColor != damageColor)
            {
                ReceiveDamage(damage);
            }
        }

        protected virtual void ReceiveDamage(float damage)
        {
            if (health <= 0)
                return;
            
            health = Mathf.Max(0, health - damage);
            
            if (health <= 0)
                Die();
            else
                spriteAnim!.SetTrigger(Hurt);
        }

        public EntityColor GetMaskColor()
        {
            return maskColor;
        }
        
        #endregion
        
        #endregion
    }
}

