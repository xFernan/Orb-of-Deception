using System;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyDamageArea : MonoBehaviour, IOrbHittable
    {
        private EnemyController _enemyController;
        
        private void Awake()
        {
            _enemyController = GetComponentInParent<EnemyController>();
        }

        public void Hit(GameEntity.EntityColor damageColor, int damage = 0)
        {
            _enemyController.GetDamaged(damageColor, damage);
        }
    }
}
