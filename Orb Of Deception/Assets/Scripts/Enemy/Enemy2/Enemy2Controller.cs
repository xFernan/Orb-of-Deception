using Pathfinding;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy2
{
    public class Enemy2Controller : EnemyController
    {
        #region Variables
        
        [Header("Enemy 2 variables")]
        [SerializeField] private float velocity = 5;
        [SerializeField] private float distanceToChase;
        [SerializeField] private float nextWaypointDistance;
        [SerializeField] private Transform spriteObject; // Provisional, se hará con animaciones,
                                                         // teniendo cada dirección su propio sombreado.
        private Transform _player;
        private Seeker _seeker;
        
        public Rigidbody2D Rigidbody { get; private set; }
        
        public const int IdleState = 0;
        public const int ChasingState = 1;

        #endregion
        
        #region Methods
        
        protected override void Awake()
        {
            base.Awake();

            Rigidbody = GetComponent<Rigidbody2D>();
            _seeker = GetComponent<Seeker>();
            _player = GameObject.FindWithTag("Player").transform; // Provisional
            
            AddState(IdleState, new IdleState(this, _player, distanceToChase));
            AddState(ChasingState, new ChasingState(this, distanceToChase, _seeker, velocity, spriteObject, _player, nextWaypointDistance));
        }

        private void Start()
        {
            SetInitialState(IdleState);
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, distanceToChase);
        }
        
        #endregion
    }
}
