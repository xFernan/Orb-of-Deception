using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Nanref.Enemy.GroundedEnemy
{
    public class GroundedEnemyController : EnemyController
    {
        #region Variables
        
        [SerializeField] private float velocity = 5;
        [SerializeField] private float chaseVelocity = 8;
        [SerializeField] private Vector2 distanceToChase;
        [SerializeField] private float distanceToAttack;
        [SerializeField] private float timeHurt = 0.5f;
        [SerializeField] private float timeToChangeDirectionAgain = 0.5f;
        [SerializeField] private float rayDetectorDistance;
        [SerializeField] private Transform leftGroundDetector;
        [SerializeField] private Transform rightGroundDetector;
        [SerializeField] private Transform[] leftWallDetectors;
        [SerializeField] private Transform[] rightWallDetectors;
        [SerializeField] private bool beginsFacingRight = true;

        private Rigidbody2D _rigidbody;
        private Transform _spriteObject;
        private Transform _player;
        
        // Constantes enteras correspondientes al ID de cada estado dentro de la FSM.
        public const int WalkingState = 0;
        public const int ChasingState = 1;
        public const int AttackingState = 2;
        public const int HurtState = 3;
        public const int DyingState = 4;

        #endregion
        
        #region Methods
        
        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteObject = GetComponentInChildren<SpriteRenderer>().transform;
            _player = GameObject.FindWithTag("Player").transform;
            
            // Se añaden los estados concretos del enemigo de a pie.
            AddState(WalkingState,
                new WalkingState(this, velocity, distanceToChase, _rigidbody, _spriteObject, rayDetectorDistance, timeToChangeDirectionAgain,
                    leftGroundDetector, rightGroundDetector, leftWallDetectors, rightWallDetectors, _player));
            AddState(ChasingState, new ChasingState(this, chaseVelocity, distanceToChase, distanceToAttack, _rigidbody, _spriteObject, _player));
            AddState(AttackingState, new AttackingState(this, distanceToAttack, _spriteObject, _player));
            AddState(HurtState, new HurtState(this, distanceToAttack, _spriteObject, _player, timeHurt));
            AddState(DyingState, new DyingState(this));
        }

        private void Start()
        {
            var localScale = _spriteObject.localScale;
            localScale.x *= beginsFacingRight ? 1 : -1;
            _spriteObject.localScale = localScale;
            
            SetInitialState(WalkingState);
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
            // Se dibujan los raycasts y demás rangos de cambios de estado, para poder ajustarlos con más comodidad desde el inspector.
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(leftGroundDetector.position, leftGroundDetector.position + Vector3.down * rayDetectorDistance);
            Gizmos.DrawLine(rightGroundDetector.position, rightGroundDetector.position + Vector3.down * rayDetectorDistance);
            
            Gizmos.color = Color.green;
            foreach (var rightWallDetector in rightWallDetectors)
            {
                Gizmos.DrawLine(rightWallDetector.position, rightWallDetector.position + Vector3.right * rayDetectorDistance);
            }
            foreach (var leftWallDetector in leftWallDetectors)
            {
                Gizmos.DrawLine(leftWallDetector.position, leftWallDetector.position + Vector3.left * rayDetectorDistance);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.right * distanceToChase.x + Vector3.up * distanceToChase.y,
                transform.position + Vector3.right * distanceToChase.x - Vector3.up * distanceToChase.y);
            Gizmos.DrawLine(transform.position + Vector3.left * distanceToChase.x + Vector3.up * distanceToChase.y,
                transform.position + Vector3.left * distanceToChase.x - Vector3.up * distanceToChase.y);
            Gizmos.DrawLine(transform.position + Vector3.right * distanceToChase.x + Vector3.up * distanceToChase.y,
                transform.position - Vector3.right * distanceToChase.x + Vector3.up * distanceToChase.y);
            Gizmos.DrawLine(transform.position + Vector3.right * distanceToChase.x - Vector3.up * distanceToChase.y,
                transform.position - Vector3.right * distanceToChase.x - Vector3.up * distanceToChase.y);
            
            Gizmos.color = new Color(1, 0.5f, 0, 1);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * distanceToAttack);
            Gizmos.DrawLine(transform.position, transform.position - Vector3.right * distanceToAttack);
        }
        
        #endregion
    }
}