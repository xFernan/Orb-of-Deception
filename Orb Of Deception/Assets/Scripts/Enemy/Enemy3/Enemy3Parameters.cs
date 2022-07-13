using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    public class Enemy3Parameters : EnemyParameters
    {
        public new Enemy3Stats Stats => stats as Enemy3Stats;

        [Header("Enemy 3 Parameters")]
        
        public float distanceToAttack;
        public float timePreparingAttack = 1;
        public float timeBetweenAttacks = 1;
        public int initialDirection = 1;
    }
}