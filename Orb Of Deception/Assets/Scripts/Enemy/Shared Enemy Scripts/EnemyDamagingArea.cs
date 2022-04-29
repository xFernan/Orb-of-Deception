using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyDamagingArea : MonoBehaviour
    {
        private Collider2D[] _colliders;
        
        private void Awake()
        {
            _colliders = GetComponents<Collider2D>();
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            var playerAreaDamage = other.GetComponent<PlayerAreaDamage>();

            if (playerAreaDamage == null) return;
            
            playerAreaDamage.ReceiveDamage();
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
