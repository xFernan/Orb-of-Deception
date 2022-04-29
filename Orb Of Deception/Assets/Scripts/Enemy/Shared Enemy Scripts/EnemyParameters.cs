using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyParameters : MonoBehaviour
    {
        [Header("Shared Enemy Parameters")] [SerializeField]
        public int id;
        [SerializeField] protected EnemyStats stats;
        public GameEntity.EntityColor maskColor;

        public bool orientationIsRight = true;

        [HideInInspector] public bool hasBeenSpawned = false;

        public EnemyStats Stats => stats;
    }
}
