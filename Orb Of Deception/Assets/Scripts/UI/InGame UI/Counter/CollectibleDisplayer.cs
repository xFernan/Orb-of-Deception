using OrbOfDeception.Rooms;

namespace OrbOfDeception.UI.InGame_UI.Counter
{
    public class CollectibleDisplayer : UIValueDisplayer
    {
        private static CollectibleDisplayer _instance;

        public static CollectibleDisplayer Instance => _instance;

        protected override void Awake()
        {
            base.Awake();
            
            _instance = this;
        }
        
        protected override float targetValueToShow => SaveSystem.GetCollectiblesAcquiredAmount();
    }
}