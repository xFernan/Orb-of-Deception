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