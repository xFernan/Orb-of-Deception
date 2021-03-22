using System.Collections.Generic;
using System.Linq;
using Nanref.Core;
using UnityEngine;

namespace Nanref.Enemy.GroundedEnemy
{
    public class WalkingState : EnemyState
    {
        #region Variables
        
        private readonly float _velocity;
        private readonly Vector2 _distanceToChase;
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _spriteObject;
        private readonly float _detectionDistance;
        private readonly float _timeToChangeDirectionAgain;
        private readonly Transform _leftGroundDetector;
        private readonly Transform _rightGroundDetector;
        private readonly Transform[] _leftWallDetectors;
        private readonly Transform[] _rightWallDetectors;
        private readonly Transform _player;
        
        private int _direction;
        private bool _canChangeDirection = true;
        private readonly MethodDelayer _canChangeDirectionAgainDelayer;

        #endregion
        
        #region Methods

        public WalkingState(EnemyController enemyController, float velocity, Vector2 distanceToChase, Rigidbody2D rigidbody, Transform spriteObject, float detectionDistance, float timeToChangeDirectionAgain, Transform leftGroundDetector, Transform rightGroundDetector, Transform[] leftWallDetectors, Transform[] rightWallDetectors, Transform player) : base(enemyController)
        {
            _velocity = velocity;
            _distanceToChase = distanceToChase;
            _rigidbody = rigidbody;
            _spriteObject = spriteObject;
            _detectionDistance = detectionDistance;
            _timeToChangeDirectionAgain = timeToChangeDirectionAgain;
            _leftGroundDetector = leftGroundDetector;
            _rightGroundDetector = rightGroundDetector;
            _leftWallDetectors = leftWallDetectors;
            _rightWallDetectors = rightWallDetectors;
            _player = player;

            // Pequeño delay para evitar que el enemigo cambie de dirección muy rápido mientras el raycast sigue colisionando con la pared por unos pocos frames.
            _canChangeDirectionAgainDelayer = new MethodDelayer(CanChangeDirectionAgain);

            animatorBoolParameterName = "IsWalking";
        }

        public override void Enter()
        {
            base.Enter();
            
            _direction = (_spriteObject.localScale.x > 0) ? 1 : -1;
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
            
            if (_canChangeDirection)
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
                var isChangingDirection = ((!isCollidingGroundRight || isCollidingWallRight) && _direction == 1) ||
                                          ((!isCollidingGroundLeft || isCollidingWallLeft) && _direction == -1);
                // En caso de que no se detecte suelo en ninguno de los pies, se da por hecho que está cayendo y por tanto tampoco
                // cambiará de dirección.
                isChangingDirection &= !(!isCollidingGroundLeft && !isCollidingGroundRight);

                if (isChangingDirection)
                {
                    ChangeDirection();
                }
            }

            var playerPosition = _player.position;
            var enemyPosition = enemyController.transform.position;
            
            var distanceFromPlayer = new Vector2(
                Mathf.Abs(playerPosition.x - enemyPosition.x),
                Mathf.Abs(playerPosition.y - enemyPosition.y)
            );
            var isLookingPlayer = Mathf.Sign(playerPosition.x - enemyPosition.x) ==
                                  Mathf.Sign(_direction);
            
            if (isLookingPlayer && distanceFromPlayer.x <= _distanceToChase.x && distanceFromPlayer.y <= _distanceToChase.y)
            {
                enemyController.SetState(GroundedEnemyController.ChasingState);
            }
        }

        private void ChangeDirection()
        {
            _direction *= -1;
            _rigidbody.velocity = new Vector2(_direction * _velocity, _rigidbody.velocity.y);
            
            var localScale = _spriteObject.localScale;
            localScale.x *= -1;
            _spriteObject.localScale = localScale;

            _canChangeDirection = false;
            _canChangeDirectionAgainDelayer.SetNewDelay(_timeToChangeDirectionAgain);
        }

        private void CanChangeDirectionAgain()
        {
            _canChangeDirection = true;
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