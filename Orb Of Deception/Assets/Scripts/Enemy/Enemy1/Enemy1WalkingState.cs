using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{

    public class WalkingState : EnemyState
    {
        #region Variables
        
        private readonly Enemy1Parameters _parameters;
        private readonly Rigidbody2D _rigidbody;
        private readonly SurroundingsDetector _surroundings;
        
        private int _direction;
        private bool _canChangeDirection = true;
        private readonly MethodDelayer _canChangeDirectionAgainDelayer;
        
        private const float TimeToChangeDirectionAgain = 0.5f;

        #endregion
        
        #region Methods

        public WalkingState(Enemy1Controller enemy) : base(enemy)
        {
            _rigidbody = enemy.Rigidbody;
            _surroundings = enemy.SurroundingsDetector;
            _parameters = enemy.Parameters;

            _direction = _parameters.hasBeenSpawned
                ? (_parameters.orientationIsRight ? 1 : -1)
                : _parameters.initialDirection;

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
                /*Debug.Log(_canChangeDirection);
                Debug.Log("RightGround:" +_surroundings.IsDetectingRightGround + " RightWall:" +
                          _surroundings.IsDetectingRightWall + " LeftGround:" + _surroundings.IsDetectingLeftGround +
                          " LeftWall:" + _surroundings.IsDetectingLeftWall);*/
                ChangeDirection();
            }
        }
        
        private void ChangeDirection()
        {
            _direction *= -1;

            _canChangeDirection = false;
            _canChangeDirectionAgainDelayer.SetNewDelay(TimeToChangeDirectionAgain);
        }

        private void CanChangeDirectionAgain()
        {
            _canChangeDirection = true;
        }
        
        private bool IsGoingToChangeDirection()
        {
            // Si está yendo hacia una dirección y se detecta que no hay suelo o que enfrente tiene una pared, cambiará de dirección.
            return
                ((!_surroundings.IsDetectingRightGround || _surroundings.IsDetectingRightWall) && _direction == 1) ||
                ((!_surroundings.IsDetectingLeftGround || _surroundings.IsDetectingLeftWall) && _direction == -1);
        }
        #endregion
    }
}