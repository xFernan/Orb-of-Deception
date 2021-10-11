using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyParameters : MonoBehaviour
    {
        [Header("Shared Enemy Parameters")]
        [SerializeField] protected EnemyStats stats;
        public GameEntity.EntityColor maskColor;

        public bool orientationIsRight = true;/*
        {
            set
            {
                orientationIsRight = value;
                enemyController?.onMaskColorChange?.Invoke();
            }
            get => orientationIsRight;
        }*/

        public EnemyStats Stats => stats;
        public EnemyController enemyController;
    }
}
