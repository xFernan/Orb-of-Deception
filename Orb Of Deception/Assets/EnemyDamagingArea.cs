using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class EnemyDamagingArea : MonoBehaviour
    {
        private Collider2D[] _colliders;
        private int _collisionDamage;
        
        private void Awake()
        {
            _colliders = GetComponents<Collider2D>();
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            var playerAreaDamage = other.GetComponent<PlayerAreaDamage>();

            if (playerAreaDamage == null) return;
            
            playerAreaDamage.ReceiveDamage(_collisionDamage);
        }

        public void SetCollisionDamage(int collisionDamage)
        {
            _collisionDamage = collisionDamage;
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
