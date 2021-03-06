using UnityEngine;

namespace OrbOfDeception.Patterns
{
    public class FiniteStateMachine
    {
        #region Variables
        
        private State _currentState;
        
        #endregion

        #region Methods

        public void SetInitialState(State initialState)
        {
            _currentState = initialState;
            _currentState.Enter();
        }

        public void SetState(State incomingState)
        {
            _currentState.Exit();
            _currentState = incomingState;
            _currentState.Enter();
        }
        
        public void ExitState()
        {
            _currentState.Exit();
            _currentState = null;
        }
        
        public void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }

        public void FixedUpdate(float deltaTime)
        {
            _currentState?.FixedUpdate(deltaTime);
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            _currentState?.OnCollisionEnter2D(other);
        }
        #endregion
    }
}
