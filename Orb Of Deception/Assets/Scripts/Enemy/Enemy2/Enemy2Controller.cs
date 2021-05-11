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

        private Rigidbody2D _rigidbody;
        private Transform _spriteObject;
        private Transform _player;
        private Seeker _seeker;
        
        public const int IdleState = 0;
        public const int ChasingState = 1;

        #endregion
        
        #region Methods
        
        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody2D>();
            _seeker = GetComponent<Seeker>();
            _spriteObject = GetComponentInChildren<SpriteRenderer>().transform;
            _player = GameObject.FindWithTag("Player").transform; // Provisional
            
            AddState(IdleState, new IdleState(this, _player, distanceToChase));
            AddState(ChasingState, new ChasingState(this, distanceToChase, _rigidbody, _seeker, velocity, _spriteObject, _player, nextWaypointDistance));
        }

        private void Start()
        {
            SetInitialState(IdleState);
        }

        protected override void Die()
        {
            base.Die();

            Destroy(gameObject);
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
