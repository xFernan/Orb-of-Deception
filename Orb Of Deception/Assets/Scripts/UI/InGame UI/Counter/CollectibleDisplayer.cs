using OrbOfDeception.Gameplay.Player;

namespace OrbOfDeception.UI.Essence_of_Punishment_Counter
{
    public class CollectibleDisplayer : UIValueDisplayer
    {
        private void Start()
        {
            GameManager.Player.CollectibleCounter.onCollectibleAcquire += ShowCounter;
        }
        
        protected override float targetValueToShow => GameManager.Player.CollectibleCounter.collectibles;
    }
}