using System.Collections.Generic;
using System.Linq;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{

    public class WalkingState : EnemyState
    {
        #region Variables
        
        private readonly float _velocity;
        private readonly Rigidbody2D _rigidbody;
        private readonly float _detectionDistance;
        private readonly float _timeToChangeDirectionAgain;
        private readonly Transform _leftGroundDetector;
        private readonly Transform _rightGroundDetector;
        private readonly Transform[] _leftWallDetectors;
        private readonly Transform[] _rightWallDetectors;
        
        private int _direction;
        private bool _canChangeDirectionBecauseOfDelay = true;
        private readonly MethodDelayer _canChangeDirectionAgainDelayer;

        #endregion
        
        #region Methods

        public WalkingState(EnemyController enemyController, float velocity, int initialDirection, float detectionDistance, float timeToChangeDirectionAgain, Transform leftGroundDetector, Transform rightGroundDetector, Transform[] leftWallDetectors, Transform[] rightWallDetectors) : base(enemyController)
        {
            var enemy = enemyController as Enemy1Controller;
            
            _velocity = velocity;
            _direction = initialDirection;
            _rigidbody = enemy.Rigidbody;
            _detectionDistance = detectionDistance;
            _timeToChangeDirectionAgain = timeToChangeDirectionAgain;
            _leftGroundDetector = leftGroundDetector;
            _rightGroundDetector = rightGroundDetector;
            _leftWallDetectors = leftWallDetectors;
            _rightWallDetectors = rightWallDetectors;

            // Pequeño delay para evitar que el enemigo cambie de dirección muy rápido mientras el raycast sigue colisionando con la pared por unos pocos frames.
            _canChangeDirectionAgainDelayer = new MethodDelayer(CanChangeDirectionAgain);
        }

        public override void Enter()
        {
            base.Enter();
            
            _rigidbody.velocity = new Vector2(_direction * _velocity, _rigidbody.velocity.y);
        }

        public override void Exit()
        {
            base.Exit();

            _rigidbody.velocity = Vector2.zero;
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            
            _canChangeDirectionAgainDelayer.Update(deltaTime);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            if (_canChangeDirectionBecauseOfDelay && IsGoingToChangeDirection())
            {
                ChangeDirection();
            }
        }

        private bool IsGoingToChangeDirection()
        {
            // Se calculan las colisiones con el suelo y las paredes desde ambas direcciones.
            
            var isCollidingWallRight = IsRaycastingToLayer(_rightWallDetectors, Vector2.right, _detectionDistance,
                LayerMask.GetMask("Ground"));
            
            var isCollidingWallLeft = IsRaycastingToLayer(_leftWallDetectors, Vector2.left, _detectionDistance,
                LayerMask.GetMask("Ground"));
            
            var isCollidingGroundRight = IsRaycastingToLayer(_rightGroundDetector, Vector2.down, _detectionDistance,
                LayerMask.GetMask("Ground"));
            
            var isCollidingGroundLeft = IsRaycastingToLayer(_leftGroundDetector, Vector2.down, _detectionDistance,
                LayerMask.GetMask("Ground"));

            // Si está yendo hacia una dirección y se detecta que no hay suelo o que enfrente tiene una pared, cambiará de dirección.
            var isGoingToChangeDirection = ((!isCollidingGroundRight || isCollidingWallRight) && _direction == 1) ||
                                           ((!isCollidingGroundLeft || isCollidingWallLeft) && _direction == -1);
            
            // En caso de que no se detecte suelo en ninguno de los pies, se da por hecho que está cayendo y por tanto tampoco
            // cambiará de dirección.
            isGoingToChangeDirection &= !(!isCollidingGroundLeft && !isCollidingGroundRight);

            return isGoingToChangeDirection;
        }
        
        private void ChangeDirection()
        {
            _direction *= -1;
            _rigidbody.velocity = new Vector2(_direction * _velocity, _rigidbody.velocity.y);

            _canChangeDirectionBecauseOfDelay = false;
            _canChangeDirectionAgainDelayer.SetNewDelay(_timeToChangeDirectionAgain);
        }

        private void CanChangeDirectionAgain()
        {
            _canChangeDirectionBecauseOfDelay = true;
        }
        
        private static bool IsRaycastingToLayer(IEnumerable<Transform> origins, Vector2 rayDirection, float maxDistance, LayerMask layerMask)
        {
            return origins.Any(origin => Physics2D.Raycast(origin.position, rayDirection, maxDistance, layerMask));
        }
        
        
        private static bool IsRaycastingToLayer(Transform origin, Vector2 rayDirection, float maxDistance, LayerMask layerMask)
        {
            return Physics2D.Raycast(origin.position, rayDirection, maxDistance, layerMask);
        }
        #endregion
    }
}