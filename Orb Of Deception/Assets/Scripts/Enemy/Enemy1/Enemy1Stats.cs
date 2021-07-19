using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy1
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStats/Enemy1")]
    public class Enemy1Stats : EnemyStats
    {
        [Header("Enemy 1 Stats")]
        public float velocity;
    }
}
