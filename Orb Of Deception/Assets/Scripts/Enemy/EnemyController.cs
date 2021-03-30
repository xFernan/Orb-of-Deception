using System;
using System.Collections.Generic;
using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public abstract class EnemyController : MonoBehaviour
    {
        
        #region Classes

        protected enum MaskColor
        {
            White,
            Black
        }
        #endregion
        
        #region Variables

        [SerializeField] protected MaskColor maskColor;
        [SerializeField] protected float health;
        
        private FiniteStateMachine _stateMachine;
        // Diccionario en el que, dado un ID, se devolverá el estado correspondiente (definido en los hijos de EnemyController).
        private Dictionary<int, EnemyState> _states;
        
        #endregion
        
        #region Properties
        
        // Permite ejecutar métodos al final de la animación mediante eventos (el método en cuestión es añadido por el estado correspondiente).
        public Action GoToNextStateCallback { set; private get; }
        public Animator Anim  { private set; get; }
        public bool IsWhite => maskColor == MaskColor.White;
        
        #endregion

        #region Methods
        
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
        
        public void SetState(int stateId)
        {
            _stateMachine.SetState(_states[stateId]);
        }

        protected void SetInitialState(int stateId)
        {
            _stateMachine.SetInitialState(_states[stateId]);
        }
        
        private void GoToNextState()
        {
            GoToNextStateCallback?.Invoke();
        }

        protected virtual void Die()
        {
            
        }
        
        public virtual void ReceiveDamage(float damage)
        {
            health = Mathf.Max(0, health - damage);
        }

        // Método que añade un estado al diccionario.
        protected void AddState(int stateId, EnemyState stateAdded)
        {
            _states.Add(stateId, stateAdded);
        }
        
        #endregion
    }
}

