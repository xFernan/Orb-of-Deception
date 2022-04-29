using System;
using OrbOfDeception.Rooms;
using OrbOfDeception.UI.InGame_UI;

namespace OrbOfDeception.Player
{
    public class PlayerHealthController
    {
        private readonly PlayerController _playerController;
        private int _health;
        
        public static Action onPlayerDamage;

        public PlayerHealthController(PlayerController playerController, int health)
        {
            _playerController = playerController;
            _health = health;
        }
        
        public void ReceiveDamage()
        {
            _health--;
            SaveSystem.AddHitToCounter();
            onPlayerDamage?.Invoke();
            
            _playerController.SpriteAnimator.ResetTrigger("Normal"); //Provisional.

            if (_health <= 0)
            {
                _playerController.DeathController.Die();
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

        public void RecoverAll()
        {
            _health = PlayerController.InitialHealth;
            InGameUIController.Instance.PlayerHealthBarController.RecoverAll();
        }
    }
}