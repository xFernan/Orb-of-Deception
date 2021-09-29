using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using Pathfinding;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy2
{
    public class ChasingState : EnemyState
    {
        #region Variables

        private readonly Enemy2Parameters _parameters;
        private readonly Rigidbody2D _rigidbody;
        private readonly Seeker _seeker;
        private readonly SpriteRenderer _spriteRenderer;
        
        private readonly Transform _transform;
        private readonly float _nextWaypointDistance;
        private Path _path;
        private int _currentWaypoint;
        private readonly MethodDelayer _updatePathDelayer;

        #endregion
        
        #region Methods

        public ChasingState(Enemy2Controller enemy,
            Seeker seeker, SpriteRenderer spriteRenderer, float nextWaypointDistance) : base(enemy)
        {
            _parameters = enemy.Parameters;
            _rigidbody = enemy.Rigidbody;
            _transform = enemy.transform;
            _seeker = seeker;
            _spriteRenderer = spriteRenderer;
            _nextWaypointDistance = nextWaypointDistance;

            _updatePathDelayer = new MethodDelayer(enemy, UpdatePath);
        }

        public override void Enter()
        {
            base.Enter();

            UpdatePath();
        }

        private void UpdatePath()
        {
            _seeker.StartPath(_rigidbody.position, PlayerGroup.Player.transform.position, OnPathComplete);
            
            // Se vuelve a llamar al método cada medio segundo para actualizar el camino en caso de haber cambiado.
            _updatePathDelayer.SetNewDelay(0.2f);
        }
        
        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentWaypoint = 0;
            }
        }

        public override void Exit()
        {
            base.Exit();

            _rigidbody.velocity = Vector2.zero;
            _updatePathDelayer.StopDelay();
        }
        
        /*public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            var distanceFromPlayer = Vector2.Distance(PlayerGroupController.Instance.playerController.transform.position, enemyController.transform.position);

            if (distanceFromPlayer > _stats.distanceToChase)
            {
                enemyController.SetState(Enemy2Controller.IdleState);
            }
        }*/

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            
            Move();
        }

        private void Move()
        {
            Vector2 forceDirection;

            var distanceFromPlayer = Vector2.Distance(PlayerGroup.Player.transform.position, _transform.position);
            
            if (distanceFromPlayer <= _parameters.distanceToIgnorePathToFollowPlayer)
            {
                forceDirection = ((Vector2) PlayerGroup.Player.transform.position - _rigidbody.position).normalized;
            }
            else
            {
                // Se sale del método si no hay camino calculado o el que hay se ha completado.
                if (_path == null || _currentWaypoint >= _path.vectorPath.Count)
                {
                    return;
                }
                
                // Si se está lo suficientemente cerca del waypoint actual, se pasa a tomar como objetivo el siguiente del path.
                var distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[_currentWaypoint]);

                if (distance < _nextWaypointDistance)
                {
                    _currentWaypoint++;
                }
                
                // SEEK: Se calculan la dirección y la fuerza a añadir a la velocidad actual.
                forceDirection = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody.position).normalized;
            }
            
            var force = forceDirection * (_parameters.Stats.velocity * Time.deltaTime);
            _rigidbody.AddForce(force);

            // Flip del sprite.
            var directionFromPlayer = Mathf.Sign(PlayerGroup.Player.transform.position.x - _transform.position.x);
            _spriteRenderer.flipX = directionFromPlayer > 0;
        }

        #endregion
    }
}