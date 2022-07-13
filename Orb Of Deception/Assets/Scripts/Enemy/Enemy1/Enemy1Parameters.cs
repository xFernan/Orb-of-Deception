using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{
    public class Enemy1Parameters : EnemyParameters
    {
        public new Enemy1Stats Stats => stats as Enemy1Stats;
        
        [Header("Enemy 1 Parameters")]
        
        public int initialDirection = 1;
    }
}
