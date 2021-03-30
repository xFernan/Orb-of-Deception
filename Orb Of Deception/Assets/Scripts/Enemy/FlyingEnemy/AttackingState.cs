using UnityEngine;

namespace OrbOfDeception.Enemy.FlyingEnemy
{
    public class AttackingState : EnemyState
    {
        #region Variables
        
        private readonly float _distanceToAttack;
        private readonly Transform _player;
        private readonly ParticleSystem _attackingParticles;

        #endregion
        
        #region Methods

        public AttackingState(EnemyController enemyController, float distanceToAttack,
            Transform player, ParticleSystem attackParticles) : base(enemyController)
        {
            _distanceToAttack = distanceToAttack;
            _player = player;
            _attackingParticles = attackParticles;
            
            animatorBoolParameterName = "IsAttacking";
        }

        public override void Enter()
        {
            base.Enter();
            
            // Se asigna el método que será llamado al final de la animación.
            enemyController.GoToNextStateCallback = GoToNextState;
            
            _attackingParticles.Play();
        }

        public override void Exit()
        {
            base.Exit();

            enemyController.GoToNextStateCallback = null;
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