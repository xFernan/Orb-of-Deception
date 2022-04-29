using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    public class Enemy3WalkingState : EnemyState
    {
        #region Variables
    
        private readonly Transform _transform;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Enemy3Parameters _parameters;
        private readonly Rigidbody2D _rigidbody;
        private readonly SurroundingsDetector _surroundings;
        private readonly Enemy3StaffController _staffController;
        private readonly float _timeToBeAbleChangeDirectionAgain;

        private int _direction;
        private bool _canChangeDirection = true;
        private readonly MethodDelayer _canChangeDirectionAgainDelayer;
        
        private const float TimeToChangeDirectionAgain = 0.5f;
        #endregion
        
        #region Methods
        public Enemy3WalkingState(Enemy3Controller enemy) : base(enemy)
        {
            animatorBoolParameterName = "IsWalking";
            
            _transform = enemy.transform;
            _spriteRenderer = enemy.SpriteRenderer;
            _rigidbody = enemy.Rigidbody;
            _parameters = enemy.Parameters;
            _surroundings = enemy.SurroundingsDetector;
            _staffController = enemy.StaffController;
            _direction = _parameters.initialDirection;
            
            // Pequeño delay para evitar que el enemigo cambie de dirección muy rápido mientras el raycast sigue
            // colisionando con la pared/sigue sin colisionar el suelo por unos pocos frames.
            _canChangeDirectionAgainDelayer = new MethodDelayer(enemy, CanChangeDirectionAgain);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            var enemy3Controller = enemy as Enemy3Controller;
            if (enemy3Controller.IsSeeingPlayer(_parameters.distanceToAttack) && GameManager.Player.isControlled)
            {
                enemy.SetState(Enemy3Controller.AttackingState);
                return;
            }
            
            _rigidbody.velocity = new Vector2(_direction * _parameters.Stats.velocity, _rigidbody.velocity.y);
            
            if (_canChangeDirection && IsGoingToChangeDirection())
            {
                ChangeDirection();
            }
            
            // Flip del sprite.
            var isOrientationRight = _direction > 0;
            _spriteRenderer.flipX = !isOrientationRight;
            _staffController.UpdateXOffset(isOrientationRight);
        }

        public override void Exit()
        {
            base.Exit();
            
            _rigidbody.velocity = Vector2.zero;
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