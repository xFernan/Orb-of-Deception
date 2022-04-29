using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStats/Enemy3")]
    public class Enemy3Stats : EnemyStats
    {
        [Header("Enemy 3 Stats")]
        public float velocity;
    }
}