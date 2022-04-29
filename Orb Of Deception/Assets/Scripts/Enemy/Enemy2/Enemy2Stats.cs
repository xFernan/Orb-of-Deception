using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy2
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStats/Enemy2")]
    public class Enemy2Stats : EnemyStats
    {
        [Header("Enemy 2 Stats")]
        public float velocityChasing;
        public float velocityWandering;
    }
}
