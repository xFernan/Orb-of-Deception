using Pathfinding;
using UnityEngine;

namespace OrbOfDeception.Enemy.FlyingEnemy
{
    public class FlyingEnemyController : EnemyController
    {
        [SerializeField] private float velocity = 5;
        [SerializeField] private float distanceToChase;
        [SerializeField] private float distanceToAttack;
        [SerializeField] private float timeHurt = 0.5f;
        [SerializeField] private float nextWaypointDistance;
        [SerializeField] private ParticleSystem attackParticles;

        private Rigidbody2D _rigidbody;
        private Transform _spriteObject;
        private Transform _player;
        private Seeker _seeker;
        
        // Constantes enteras correspondientes al ID de cada estado dentro de la FSM.
        public const int IdleState = 0;
        public const int ChasingState = 1;
        public const int AttackingState = 2;
        public const int HurtState = 3;
        public const int DyingState = 4;

        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody2D>();
            _seeker = GetComponent<Seeker>();
            _spriteObject = GetComponentInChildren<SpriteRenderer>().transform;
            _player = GameObject.FindWithTag("Player").transform;
            
            // Se añaden los estados concretos del enemigo volador.
            AddState(IdleState,
                new IdleState(this, _player, distanceToChase));
            AddState(ChasingState, new ChasingState(this, distanceToChase, distanceToAttack, _rigidbody, _seeker, velocity, _spriteObject, _player, nextWaypointDistance));
            AddState(AttackingState, new AttackingState(this, distanceToAttack, _player, attackParticles));
            AddState(HurtState, new HurtState(this, distanceToAttack, _player, timeHurt));
            AddState(DyingState, new DyingState(this));
        }

        private void Start()
        {
            SetInitialState(IdleState);
        }

        public override void ReceiveDamage(float damage)
        {
            base.ReceiveDamage(damage);

            SetState(health > 0 ? HurtState : DyingState);
        }

        protected override void Die()
        {
            base.Die();

            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(position, distanceToChase);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, distanceToAttack);
        }
    }
}