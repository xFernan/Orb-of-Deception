using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace OrbOfDeception.Enemy.GroundedEnemy
{
    public class ChasingState : EnemyState
    {
        #region Variables
        
        private readonly float _velocity;
        private readonly Vector2 _distanceToChase;
        private readonly float _distanceToAttack;
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _spriteObject;
        private readonly Transform _player;
        
        private int _direction;

        #endregion
        
        #region Methods

        public ChasingState(EnemyController enemyController, float velocity, Vector2 distanceToChase,
            float distanceToAttack, Rigidbody2D rigidbody, Transform spriteObject, Transform player) : base(
            enemyController)
        {
            _velocity = velocity;
            _distanceToChase = distanceToChase;
            _distanceToAttack = distanceToAttack;
            _rigidbody = rigidbody;
            _spriteObject = spriteObject;
            _player = player;

            animatorBoolParameterName = "IsChasing";
        }

        public override void Enter()
        {
            base.Enter();
            
            // La dirección hacia la que empieza a moverse dependerá de la dirección que tiene el sprite al comienzo del movimiento.
            _direction = (_spriteObject.localScale.x > 0) ? 1 : -1;
            _rigidbody.velocity = new Vector2(_direction * _velocity, _rigidbody.velocity.y);
        }

        public override void Exit()
        {
            base.Exit();

            _rigidbody.velocity = Vector2.zero;
        }
        
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var playerPosition = _player.position;
            var enemyPosition = enemyController.transform.position;
            
            var distanceFromPlayer = new Vector2(
                Mathf.Abs(playerPosition.x - enemyPosition.x),
                Mathf.Abs(playerPosition.y - enemyPosition.y)
                );
            var isLookingPlayer = Mathf.Sign(playerPosition.x - enemyPosition.x) ==
                                  Mathf.Sign(_direction);

            if (!isLookingPlayer)
            {
                if (distanceFromPlayer.x > _distanceToChase.x || distanceFromPlayer.y > _distanceToChase.y)
                {
                    enemyController.SetState(GroundedEnemyController.WalkingState);
                }
            }
            else if (distanceFromPlayer.x <= _distanceToAttack)
            {
                enemyController.SetState(GroundedEnemyController.AttackingState);
            }
        }
        #endregion
    }
}