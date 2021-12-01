using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{
    public class Enemy1Controller : EnemyController
    {
        #region Variables

        [Header("Enemy 1 variables")]
        [SerializeField] private int initialDirection = 1;
        [SerializeField] private float timeToChangeDirectionAgain = 0.5f;
        
        public Rigidbody2D Rigidbody { get; private set; }
        public SurroundingsDetector SurroundingsDetector { get; private set; }
        public Enemy1Parameters Parameters => parameters as Enemy1Parameters;
        
        public const int WalkingState = 0;

        #endregion
        
        #region Methods
        
        protected override void OnAwake()
        {
            base.OnAwake();

            Rigidbody = GetComponent<Rigidbody2D>();
            SurroundingsDetector = GetComponentInChildren<SurroundingsDetector>();
            
            AddState(WalkingState,
                new WalkingState(this, initialDirection, timeToChangeDirectionAgain));
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            SetInitialState(WalkingState);
        }
        
        #endregion
    }
}
