using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{
    public class Enemy1Controller : EnemyController
    {
        #region Variables
        
        public Rigidbody2D Rigidbody { get; private set; }
        public SurroundingsDetector SurroundingsDetector { get; private set; }
        public Enemy1Parameters Parameters => BaseParameters as Enemy1Parameters;
        
        public const int WalkingState = 0;

        #endregion
        
        #region Methods
        
        protected override void OnAwake()
        {
            base.OnAwake();

            Rigidbody = GetComponent<Rigidbody2D>();
            SurroundingsDetector = GetComponentInChildren<SurroundingsDetector>();
            
            AddState(WalkingState,
                new WalkingState(this));
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            SetInitialState(WalkingState);
        }

        private void PlayStepSound()
        {
            soundsPlayer.Play("Step");
        }
        
        #endregion
    }
}
