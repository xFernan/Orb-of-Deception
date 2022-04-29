using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    public class Enemy3AttackState : EnemyState
    {
        #region Variables

        private readonly Transform _transform;
        private readonly Enemy3Parameters _parameters;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Enemy3StaffController _staffController;

        private readonly MethodDelayer _canAlreadyStopAttackingDelayer;
        private bool canStopAttacking;
        private const float TimeBeforeStopAttacking = 0.5f;
        #endregion

        #region Methods
        
        public Enemy3AttackState(Enemy3Controller enemy) : base(enemy)
        {
            animatorBoolParameterName = "IsAttacking";
            
            _transform = enemy.transform;
            _parameters = enemy.Parameters;
            _staffController = enemy.StaffController;
            _spriteRenderer = enemy.SpriteRenderer;
            
            _canAlreadyStopAttackingDelayer = new MethodDelayer(enemy, CanAlreadyStopAttacking);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Flip del sprite.
            var directionFromPlayer = Mathf.Sign(GameManager.Player.transform.position.x - _transform.position.x);
            var isOrientationRight = directionFromPlayer > 0;
            _spriteRenderer.flipX = !isOrientationRight;
            _staffController.UpdateXOffset(isOrientationRight);
            
            if (_parameters.hasBeenSpawned) return;

            if (!GameManager.Player.isControlled)
            {
                enemy.SetState(Enemy3Controller.IdleState);
            }
            else if (!IsSeeingPlayer())
            {
                enemy.SetState(Enemy3Controller.IdleState);
            }/*
                if (canStopAttacking)
                {
                    enemy.SetState(Enemy3Controller.IdleState);
                }
                else if (!_canAlreadyStopAttackingDelayer.AlreadyHasADelay())
                {
                    _canAlreadyStopAttackingDelayer.SetNewDelay(TimeBeforeStopAttacking);
                }
            }
            else
            {
                _canAlreadyStopAttackingDelayer.StopDelay();
            }*/
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _staffController.EnterAttack();
            canStopAttacking = false;
        }

        public override void Exit()
        {
            base.Exit();
            
            _staffController.StopAttack();
            _canAlreadyStopAttackingDelayer.StopDelay();
        }

        private bool IsSeeingPlayer()
        {
            var enemy3Controller = enemy as Enemy3Controller;
            return enemy3Controller.IsSeeingPlayer(_parameters.distanceToAttack);
        }

        public void CanAlreadyStopAttacking()
        {
            canStopAttacking = true;
            Debug.Log("can stop attacking");
        }

        #endregion
    }
}