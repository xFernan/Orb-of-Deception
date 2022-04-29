using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy2
{
    public class WanderingState : EnemyState
    {
        #region Variables
    
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly Enemy2Parameters _parameters;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly PathController _pathController;

        #endregion
    
        #region Methods

        public WanderingState(Enemy2Controller enemy) : base(enemy)
        {
            animatorBoolParameterName = "IsWandering";
            
            _transform = enemy.transform;
            _rigidbody = enemy.Rigidbody;
            _parameters = enemy.Parameters;
            _spriteRenderer = enemy.SpriteRenderer;
            _pathController = enemy.PathController;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var position = _transform.position;
            
            var distanceFromPlayer = Vector2.Distance(GameManager.Player.transform.position, position);

            if (_pathController != null)
            {
                // Flip del sprite.
                var directionFromWaypoint = Mathf.Sign(_pathController.GetCurrentWaypointPosition().x - position.x);
                _spriteRenderer.flipX = directionFromWaypoint > 0;
            }
            
            if (GameManager.Player.isControlled && distanceFromPlayer <= _parameters.distanceToChase && enemy.IsSeeingPlayer(_parameters.distanceToChase))
            {
                enemy.SetState(Enemy2Controller.ChasingState);
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);

            if (_pathController == null)
                return;
            
            Move();
        }
        
        private void Move()
        {
            var waypointPosition = _pathController.GetCurrentWaypointPosition();

            if (Vector2.Distance(waypointPosition, _transform.position) <= 0.1f)
            {
                _pathController.GoToNextWaypoint();
                waypointPosition = _pathController.GetCurrentWaypointPosition();
            }
                
            Vector2 forceDirection = (_pathController.GetCurrentWaypointPosition() - _transform.position).normalized;
            
            var force = forceDirection * (_parameters.Stats.velocityWandering * Time.deltaTime);
            _rigidbody.AddForce(force);
        }
        #endregion
    }
}