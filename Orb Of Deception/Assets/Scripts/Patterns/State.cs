using UnityEngine;

namespace OrbOfDeception.Patterns
{
    public abstract class State
    {
        #region Methods
        
        public virtual void Enter()
        {

        }

        public virtual void Update(float deltaTime)
        {

        }

        public virtual void FixedUpdate(float deltaTime)
        {

        }
        
        public virtual void Exit()
        {

        }

        public virtual void OnCollisionEnter2D(Collision2D other)
        {
            
        }
        #endregion
    }
}