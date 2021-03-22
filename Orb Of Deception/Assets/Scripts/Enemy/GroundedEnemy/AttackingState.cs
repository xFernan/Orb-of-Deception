using UnityEngine;

namespace Nanref.Enemy.GroundedEnemy
{
    public class AttackingState : EnemyState
    {
        #region Variables
        
        private readonly float _distanceToAttack;
        private readonly Transform _spriteObject;
        private readonly Transform _player;
        
        private int _direction;

        #endregion
        
        #region Methods

        public AttackingState(EnemyController enemyController, float distanceToAttack, Transform spriteObject, Transform player) : base(enemyController)
        {
            _distanceToAttack = distanceToAttack;
            _spriteObject = spriteObject;
            _player = player;
            
            animatorBoolParameterName = "IsAttacking";
        }

        public override void Enter()
        {
            base.Enter();

            _direction = (_spriteObject.localScale.x > 0) ? 1 : -1;
            
            enemyController.GoToNextStateCallback = GoToNextState;
        }

        public override void Exit()
        {
            base.Exit();

            enemyController.GoToNextStateCallback = null;
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