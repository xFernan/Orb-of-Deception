namespace OrbOfDeception.Gameplay.Player
{
    public class KneelController
    {
        public bool isKneeling;
        public void Kneel()
        {
            isKneeling = true;
            var player = GameManager.Player;
            player.SetState(PlayerController.KneelState);
        }
        
        public void Stand()
        {
            var player = GameManager.Player;
            player.SetState(PlayerController.InControlState);
            isKneeling = false;
        }
    }
}