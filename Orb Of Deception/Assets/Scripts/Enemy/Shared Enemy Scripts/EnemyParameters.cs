using System;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyParameters : MonoBehaviour
    {
        [Header("Shared Enemy Parameters")] [SerializeField]
        public int id;
        [SerializeField] protected EnemyStats stats;
        [SerializeField] private GameEntity.EntityColor maskColor;
        public bool doesDropEssences = true;

        public GameEntity.EntityColor MaskColor
        {
            get => maskColor;
            set
            {
                if (maskColor == value) return;
                maskColor = value;
                onMaskColorChange?.Invoke();
            }
        }
        
        public Action onMaskColorChange;

        public bool orientationIsRight = true;

        [HideInInspector] public bool hasBeenSpawned = false;

        public EnemyStats Stats => stats;
    }
}
