namespace OrbOfDeception.Enemy.GroundedEnemy
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