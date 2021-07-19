using System.Collections.Generic;
using System.Linq;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{

    public class WalkingState : EnemyState
    {
        #region Variables
        
        private readonly Enemy1Parameters _parameters;
        private readonly Rigidbody2D _rigidbody;
        private readonly float _detectionDistance;
        private readonly float _timeToBeAbleChangeDirectionAgain;
        private readonly Transform _leftGroundDetector;
        private readonly Transform _rightGroundDetector;
        private readonly Transform[] _leftWallDetectors;
        private readonly Transform[] _rightWallDetectors;
        
        private int _direction;
        private bool _canChangeDirection = true;
        private readonly MethodDelayer _canChangeDirectionAgainDelayer;

        #endregion
        
        #region Methods

        public WalkingState(Enemy1Controller enemy, int startingDirection, float detectionDistance, float timeToBeAbleChangeDirectionAgain, Transform leftGroundDetector, Transform rightGroundDetector, Transform[] leftWallDetectors, Transform[] rightWallDetectors) : base(enemy)
        {
            _rigidbody = enemy.Rigidbody;
            _parameters = enemy.Parameters;
            _direction = startingDirection;
            _detectionDistance = detectionDistance;
            _timeToBeAbleChangeDirectionAgain = timeToBeAbleChangeDirectionAgain;
            _leftGroundDetector = leftGroundDetector;
            _rightGroundDetector = rightGroundDetector;
            _leftWallDetectors = leftWallDetectors;
            _rightWallDetectors = rightWallDetectors;

            // Pequeño delay para evitar que el enemigo cambie de dirección muy rápido mientras el raycast sigue
            // colisionando con la pared/sigue sin colisionar el suelo por unos pocos frames.
            _canChangeDirectionAgainDelayer = new MethodDelayer(enemy, CanChangeDirectionAgain);
        }

        public override void Exit()
        {
            base.Exit();

            _rigidbody.velocity = Vector2.zero;
            _canChangeDirectionAgainDelayer.StopDelay();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            _rigidbody.velocity = new Vector2(_direction * _parameters.Stats.velocity, _rigidbody.velocity.y);
            
            if (_canChangeDirection && IsGoingToChangeDirection())
            {
                ChangeDirection();
            }
        }
        
        private void ChangeDirection()
        {
            _direction *= -1;

            _canChangeDirection = false;
            _canChangeDirectionAgainDelayer.SetNewDelay(_timeToBeAbleChangeDirectionAgain);
        }

        private void CanChangeDirectionAgain()
        {
            _canChangeDirection = true;
        }
        
        #region MOVER LO SIGUIENTE A UN SCRIPT APARTE (TIPO MOTION LERPER).
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
        
        private static bool IsRaycastingToLayer(IEnumerable<Transform> origins, Vector2 rayDirection, float maxDistance, LayerMask layerMask)
        {
            return origins.Any(origin => Physics2D.Raycast(origin.position, rayDirection, maxDistance, layerMask));
        }
        
        
        private static bool IsRaycastingToLayer(Transform origin, Vector2 rayDirection, float maxDistance, LayerMask layerMask)
        {
            return Physics2D.Raycast(origin.position, rayDirection, maxDistance, layerMask);
        }
        #endregion
        #endregion
    }
}