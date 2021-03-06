using System.Collections.ObjectModel;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyDamageableArea : MonoBehaviour, IOrbHittable
    {
        private EnemyController _enemyController;
        private Collider2D[] _colliders;
        
        private void Awake()
        {
            _enemyController = GetComponentInParent<EnemyController>();
            _colliders = GetComponents<Collider2D>();
        }

        public void OnOrbHitEnter(GameEntity.EntityColor damageColor, int damage = 0)
        {
            _enemyController.GetDamaged(damageColor, damage);
        }

        public void ActivateCollider()
        {
            foreach (var col in _colliders)
            {
                col.enabled = true;
            }
        }
        public void DisableCollider()
        {
            foreach (var col in _colliders)
            {
                col.enabled = false;
            }
        }
    }
}
