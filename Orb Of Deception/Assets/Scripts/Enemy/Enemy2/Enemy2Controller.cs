using OrbOfDeception.Core;
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
        
        public PathController PathController { get; private set; }
        
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        
        public Enemy2Parameters Parameters => BaseParameters as Enemy2Parameters;
        
        public const int IdleState = 0;
        public const int ChasingState = 1;

        #endregion
        
        #region Methods
        
        protected override void OnAwake()
        {
            base.OnAwake();

            PathController = GetComponentInChildren<PathController>();
            if (PathController != null)
                PathController.transform.parent = null;
            
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _seeker = GetComponent<Seeker>();
            
            AddState(IdleState, new WanderingState(this));
            AddState(ChasingState, new ChasingState(this, _seeker, SpriteRenderer, nextWaypointDistance));
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (BaseParameters.hasBeenSpawned)
            {
                SetInitialState(ChasingState);
            }
            else
            {
                SetInitialState(IdleState);
            }
        }
        
        public override void SetOrientation(bool isOrientationRight)
        {
            SpriteRenderer.flipX = isOrientationRight; // Provisional, hacer con animaciones para cambiar sombras.
        }

        private void PlayFlyingSound()
        {
            //soundsPlayer.Play("Flying");
        }
        #endregion
    }
}
