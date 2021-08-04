using Pathfinding;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy2
{
    public class Enemy2Controller : EnemyController
    {
        #region Variables

        [Header("Enemy 2 variables")]
        [SerializeField] private float nextWaypointDistance;
        
        private Seeker _seeker;
        
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        
        public Enemy2Parameters Parameters => parameters as Enemy2Parameters;
        
        public const int IdleState = 0;
        public const int ChasingState = 1;

        #endregion
        
        #region Methods
        
        protected override void OnAwake()
        {
            base.OnAwake();

            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _seeker = GetComponent<Seeker>();
            
            AddState(IdleState, new IdleState(this));
            AddState(ChasingState, new ChasingState(this, _seeker, SpriteRenderer, nextWaypointDistance));
        }

        private void Start()
        {
            SetInitialState(IdleState);
        }

        private void OnDrawGizmos()
        {
            if (parameters == null) return;
            var position = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, Parameters.distanceToChase);
        }
        
        #endregion
    }
}
