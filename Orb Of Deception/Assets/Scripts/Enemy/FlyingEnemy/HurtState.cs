using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.FlyingEnemy
{
    public class HurtState : EnemyState
    {
        #region Variables
        
        private readonly float _distanceToAttack;
        private readonly Transform _player;
        private readonly float _timeHurt;
        
        private readonly MethodDelayer _nextStateDelayer;

        #endregion
        
        #region Methods

        public HurtState(EnemyController enemyController, float distanceToAttack, Transform player, float timeHurt) : base(enemyController)
        {
            _distanceToAttack = distanceToAttack;
            _player = player;
            _timeHurt = timeHurt;
            
            _nextStateDelayer = new MethodDelayer(GoToNextState);
            
            animatorBoolParameterName = "IsHurt";
        }

        public override void Enter()
        {
            base.Enter();

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
            var distanceFromPlayer = Vector2.Distance(_player.position, enemyController.transform.position);

            enemyController.SetState(distanceFromPlayer <= _distanceToAttack ?
                FlyingEnemyController.AttackingState : FlyingEnemyController.IdleState);
        }
        
        #endregion
    }
}