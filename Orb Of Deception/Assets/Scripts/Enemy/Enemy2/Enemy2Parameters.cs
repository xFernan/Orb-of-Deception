using System;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy2
{
    public class Enemy2Parameters : EnemyParameters
    {
        [Header("Enemy 2 Parameters")]
        public float distanceToChase;
        public float distanceToIgnorePathToFollowPlayer = 3;
        
        public new Enemy2Stats Stats => stats as Enemy2Stats;

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, distanceToChase);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, distanceToIgnorePathToFollowPlayer);
        }
    }
}
