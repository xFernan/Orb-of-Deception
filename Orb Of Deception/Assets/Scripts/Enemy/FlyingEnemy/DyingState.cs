namespace OrbOfDeception.Enemy.FlyingEnemy
{
    public class DyingState : EnemyState
    {
        
        #region Methods

        public DyingState(EnemyController enemyController) : base(enemyController)
        {
            animatorBoolParameterName = "IsDying";
        }
        
        #endregion
    }
}