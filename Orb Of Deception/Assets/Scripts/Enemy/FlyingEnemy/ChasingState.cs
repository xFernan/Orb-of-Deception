using OrbOfDeception.Core;
using Pathfinding;
using UnityEngine;

namespace OrbOfDeception.Enemy.FlyingEnemy
{
    public class ChasingState : EnemyState
    {
        #region Variables
        
        private readonly float _distanceToChase;
        private readonly float _distanceToAttack;
        private readonly Rigidbody2D _rigidbody;
        private readonly Seeker _seeker;
        private readonly Transform _spriteObject;
        
        private readonly float _velocity;
        private readonly Transform _player;
        private readonly float _nextWaypointDistance;
        private Path _path;
        private int _currentWaypoint;
        private readonly MethodDelayer _updatePathDelayer;

        #endregion
        
        #region Methods

        public ChasingState(EnemyController enemyController, float distanceToChase, float distanceToAttack,
            Rigidbody2D rigidbody, Seeker seeker, float velocity, Transform spriteObject, Transform player, float nextWaypointDistance) : base(
            enemyController)
        {
            _distanceToChase = distanceToChase;
            _distanceToAttack = distanceToAttack;
            _rigidbody = rigidbody;
            _seeker = seeker;
            _velocity = velocity;
            _spriteObject = spriteObject;
            _player = player;
            _nextWaypointDistance = nextWaypointDistance;

            _updatePathDelayer = new MethodDelayer(UpdatePath);

            animatorBoolParameterName = "IsChasing";
        }

        public override void Enter()
        {
            base.Enter();

            UpdatePath();
        }

        private void UpdatePath()
        {
            _seeker.StartPath(_rigidbody.position, _player.position, OnPathComplete);
            
            // Se vuelve a llamar al método cada medio segundo para actualizar el camino en caso de haber cambiado.
            _updatePathDelayer.SetNewDelay(0.5f);
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
        }
        
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            var distanceFromPlayer = Vector2.Distance(_player.position, enemyController.transform.position);

            if (distanceFromPlayer <= _distanceToAttack)
            {
                enemyController.SetState(FlyingEnemyController.AttackingState);
            }
            else if (distanceFromPlayer > _distanceToChase)
            {
                enemyController.SetState(FlyingEnemyController.IdleState);
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            
            Move();
            
            _updatePathDelayer.Update(deltaTime);
        }

        private void Move()
        {
            // Se sale del método si no hay camino calculado o el que hay se ha completado.
            if (_path == null || _currentWaypoint >= _path.vectorPath.Count)
            {
                return;
            }

            // SEEK: Se calculan la dirección y la fuerza a añadir a la velocidad actual.
            var direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody.position).normalized;
            var force = direction * (_velocity * Time.deltaTime);

            _rigidbody.AddForce(force);
            
            // Si se está lo suficientemente cerca del waypoint actual, se pasa a tomar como objetivo el siguiente del path.
            var distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[_currentWaypoint]);

            if (distance < _nextWaypointDistance)
            {
                _currentWaypoint++;
            }

            // Flip del sprite.
            var directionFromPlayer = Mathf.Sign(_player.transform.position.x - enemyController.transform.position.x);
            
            if (directionFromPlayer > 0)
            {
                _spriteObject.localScale = new Vector3(1, 1, 1);
            }
            else if (directionFromPlayer < 0)
            {
                _spriteObject.localScale = new Vector3(-1, 1, 1);
            }
        }

        #endregion
    }
}