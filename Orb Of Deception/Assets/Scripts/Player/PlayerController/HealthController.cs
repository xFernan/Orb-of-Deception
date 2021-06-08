namespace OrbOfDeception.Player
{
    public class HealthController
    {
        private readonly PlayerController _playerController;
        private int _health;

        public HealthController(PlayerController playerController, int health)
        {
            _playerController = playerController;
            _health = health;
        }
        
        public void ReceiveDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _playerController.Die();
            }
            else
            {
                _playerController.HurtController.StartHurt();
            }
        }
    }
}