using System;
using System.Collections.Generic;
using OrbOfDeception.CameraBehaviours;
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
        [SerializeField] protected EntityColor maskColor;
        [SerializeField] protected float health;

        private EnemyDamageableArea[] _damageAreas;
        
        private FiniteStateMachine _stateMachine;
        private Dictionary<int, EnemyState> _states;
        
        private static readonly int BeingHurt = Animator.StringToHash("Hurt");
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
            _damageAreas = GetComponentsInChildren<EnemyDamageableArea>();
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
            
            if (playerAreaDamage == null) return;
            
            playerAreaDamage.ReceiveDamage(10);
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
            
            foreach (var damageArea in _damageAreas)
            {
                damageArea.DisableCollider();
            }

            HideShadows();
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
            if (maskColor != damageColor)
            {
                ReceiveDamage(damage);
            }
        }

        protected virtual void ReceiveDamage(float damage)
        {
            if (health <= 0)
                return;
            
            Camera.main.GetComponentInParent<CameraController>().Shake(0.2f, 0.1f); // Provisional.
            health = Mathf.Max(0, health - damage);

            if (health <= 0)
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
            return maskColor;
        }
        
        #endregion
        
        #endregion
    }
}

