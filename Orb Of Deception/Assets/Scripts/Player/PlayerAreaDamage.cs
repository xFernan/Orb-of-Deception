using UnityEngine;

namespace OrbOfDeception.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerAreaDamage : MonoBehaviour
    {
        private PlayerController _playerController;
        
        private void Awake()
        {
            _playerController = transform.root.GetComponentInChildren<PlayerController>();
        }

        public void ReceiveDamage(int damage)
        {
            _playerController.GetDamaged(damage);
        }
    }
}
