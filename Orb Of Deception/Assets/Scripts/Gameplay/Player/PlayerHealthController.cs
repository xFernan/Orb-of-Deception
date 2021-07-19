using System;

namespace OrbOfDeception.Gameplay.Player
{
    public class PlayerHealthController
    {
        private readonly PlayerController _playerController;
        private int _health;
        
        public static Action<int> onHealthChange;

        public PlayerHealthController(PlayerController playerController, int health)
        {
            _playerController = playerController;
            _health = health;
        }
        
        public void ReceiveDamage(int damage)
        {
            _health -= damage;
            onHealthChange(_health);
            
            if (_health <= 0)
            {
                _playerController.Die();
            }
            else
            {
                _playerController.HurtController.StartHurt();
            }
        }

        public int GetCurrentHealth()
        {
            return _health;
        }
    }
}