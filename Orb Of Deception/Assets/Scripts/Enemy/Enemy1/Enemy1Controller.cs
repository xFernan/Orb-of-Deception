using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{
    public class Enemy1Controller : EnemyController
    {
        #region Variables
        
        [Header("Enemy 1 variables")]
        [SerializeField] private float velocity = 5;
        [SerializeField] private int initialDirection = 1;
        [SerializeField] private float timeToChangeDirectionAgain = 0.5f;
        [SerializeField] private float rayDetectorDistance;
        [SerializeField] private Transform leftGroundDetector;
        [SerializeField] private Transform rightGroundDetector;
        [SerializeField] private Transform[] leftWallDetectors;
        [SerializeField] private Transform[] rightWallDetectors;

        public Rigidbody2D Rigidbody { get; private set; }
        
        public const int WalkingState = 0;

        #endregion
        
        #region Methods
        
        protected override void Awake()
        {
            base.Awake();

            Rigidbody = GetComponent<Rigidbody2D>();
            
            AddState(WalkingState,
                new WalkingState(this, velocity, initialDirection, rayDetectorDistance, timeToChangeDirectionAgain,
                    leftGroundDetector, rightGroundDetector, leftWallDetectors, rightWallDetectors));
        }

        private void Start()
        {
            SetInitialState(WalkingState);
        }
        
        private void OnDrawGizmos()
        {
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
        }
        
        #endregion
    }
}
