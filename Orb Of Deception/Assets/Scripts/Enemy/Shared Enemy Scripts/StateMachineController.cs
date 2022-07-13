using System;
using System.Collections.Generic;
using OrbOfDeception.Patterns;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class StateMachineController : MonoBehaviour
    {
        protected FiniteStateMachine stateMachine;
        private Dictionary<int, State> states;

        #region MonoBehaviour Methods
        
        private void Awake()
        {
            stateMachine = new FiniteStateMachine();
            states = new Dictionary<int, State>();
            
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
            stateMachine?.Update(Time.deltaTime);
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            
        }
        
        private void FixedUpdate()
        {
            stateMachine?.FixedUpdate(Time.deltaTime);
            OnUpdate();
        }

        protected virtual void OnFixedUpdate()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            stateMachine?.OnCollisionEnter2D(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            stateMachine?.OnTriggerEnter2D(other);
        }
        
        private void OnCollisionStay2D(Collision2D other)
        {
            stateMachine?.OnCollisionStay2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            stateMachine?.OnTriggerStay2D(other);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            stateMachine?.OnCollisionExit2D(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            stateMachine?.OnTriggerExit2D(other);
        }

        #endregion
        
        #region State Machine Methods
        
        public void SetState(int stateId)
        {
            stateMachine.SetState(states[stateId]);
        }

        protected void SetInitialState(int stateId)
        {
            stateMachine.SetInitialState(states[stateId]);
        }
        
        protected void AddState(int stateId, State stateAdded)
        {
            states.Add(stateId, stateAdded);
        }
        
        #endregion
    }
}