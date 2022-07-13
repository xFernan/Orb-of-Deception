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
        
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            
        }
        
        public virtual void OnCollisionStay2D(Collision2D other)
        {
            
        }
        
        public virtual void OnTriggerStay2D(Collider2D other)
        {
            
        }
        
        public virtual void OnCollisionExit2D(Collision2D other)
        {
            
        }
        
        public virtual void OnTriggerExit2D(Collider2D other)
        {
            
        }
        #endregion
    }
}