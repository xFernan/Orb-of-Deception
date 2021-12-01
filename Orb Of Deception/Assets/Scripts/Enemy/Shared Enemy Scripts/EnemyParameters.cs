using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyParameters : MonoBehaviour
    {
        [Header("Shared Enemy Parameters")]
        [SerializeField] protected EnemyStats stats;
        public GameEntity.EntityColor maskColor;

        public bool orientationIsRight = true;

        public EnemyStats Stats => stats;
        public EnemyController enemyController;
    }
}
