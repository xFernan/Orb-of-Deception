using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.GroundedEnemy
{
    public class HurtState : EnemyState
    {
        #region Variables
        
        private readonly float _distanceToAttack;
        private readonly Transform _spriteObject;
        private readonly Transform _player;
        private readonly float _timeHurt;
        
        private int _direction;
        private readonly MethodDelayer _nextStateDelayer;

        #endregion
        
        #region Methods

        public HurtState(EnemyController enemyController, float distanceToAttack, Transform spriteObject, Transform player, float timeHurt) : base(enemyController)
        {
            _distanceToAttack = distanceToAttack;
            _spriteObject = spriteObject;
            _player = player;
            _timeHurt = timeHurt;
            
            _nextStateDelayer = new MethodDelayer(GoToNextState);
            
            animatorBoolParameterName = "IsHurt";
        }

        public override void Enter()
        {
            base.Enter();

            _direction = (_spriteObject.localScale.x > 0) ? 1 : -1;
            
            // Se aplica un delay para el cual el enemigo podrá volver a moverse/atacar.
            _nextStateDelayer.SetNewDelay(_timeHurt);
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);
            
            _nextStateDelayer.Update(deltaTime);
        }

        private void GoToNextState()
        {
            var distanceFromPlayer = Mathf.Abs(_player.position.x - enemyController.transform.position.x);
            var isLookingPlayer = Mathf.Sign(_player.position.x - enemyController.transform.position.x) ==
                                  Mathf.Sign(_direction);

            if (isLookingPlayer && distanceFromPlayer <= _distanceToAttack)
            {
                enemyController.SetState(GroundedEnemyController.AttackingState);
            }
            else
            {
                enemyController.SetState(GroundedEnemyController.WalkingState);
            }
        }
        
        #endregion
    }
}