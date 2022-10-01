using System;
using System.Collections.Generic;
using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class StateMachineController : MonoBehaviour
    {
        private FiniteStateMachine _stateMachine;
        private Dictionary<int, State> _states;

        #region MonoBehaviour Methods
        
        private void Awake()
        {
            _stateMachine = new FiniteStateMachine();
            _states = new Dictionary<int, State>();
            
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
            OnUpdate();
        }

        protected virtual void OnFixedUpdate()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _stateMachine?.OnCollisionEnter2D(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _stateMachine?.OnTriggerEnter2D(other);
        }
        
        private void OnCollisionStay2D(Collision2D other)
        {
            _stateMachine?.OnCollisionStay2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _stateMachine?.OnTriggerStay2D(other);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            _stateMachine?.OnCollisionExit2D(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _stateMachine?.OnTriggerExit2D(other);
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
        
        protected void ExitState()
        {
            _stateMachine.ExitState();
        }
        
        protected void AddState(int stateId, State stateAdded)
        {
            _states.Add(stateId, stateAdded);
        }
        
        #endregion
    }
}