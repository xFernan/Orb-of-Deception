using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Nanref.Enemy.FlyingEnemy
{
    public class IdleState : EnemyState
    {
        #region Variables
        
        private readonly Transform _player;
        private readonly float _distanceToChase;
        
        private int _direction;

        #endregion
        
        #region Methods

        public IdleState(EnemyController enemyController, Transform player, float distanceToChase) : base(enemyController)
        {
            _player = player;
            _distanceToChase = distanceToChase;
            
            animatorBoolParameterName = "IsIdle";
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var distanceFromPlayer = Vector2.Distance(_player.position, enemyController.transform.position);

            if (distanceFromPlayer <= _distanceToChase)
            {
                enemyController.SetState(FlyingEnemyController.ChasingState);
            }
        }
        #endregion
    }
}