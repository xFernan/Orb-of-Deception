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

        public void Update(float deltaTime)
        {
            _currentState.Update(deltaTime);
        }

        public void FixedUpdate(float deltaTime)
        {
            _currentState.FixedUpdate(deltaTime);
        }
        
        #endregion
    }
}
