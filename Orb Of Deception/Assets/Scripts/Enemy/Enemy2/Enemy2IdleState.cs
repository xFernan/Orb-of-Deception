using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy2
{
    public class IdleState : EnemyState
    {
        #region Variables
    
        private readonly Transform _transform;
        private readonly Enemy2Parameters _parameters;

        #endregion
    
        #region Methods

        public IdleState(Enemy2Controller enemy) : base(enemy)
        {
            _transform = enemy.transform;
            _parameters = enemy.Parameters;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var distanceFromPlayer = Vector2.Distance(PlayerGroup.Player.transform.position, _transform.position);

            if (distanceFromPlayer <= _parameters.distanceToChase)
            {
                enemyController.SetState(Enemy2Controller.ChasingState);
            }
        }
        #endregion
    }
}