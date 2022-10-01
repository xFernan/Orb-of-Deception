using OrbOfDeception.Core;
using OrbOfDeception.Orb;
using OrbOfDeception.Rooms;

namespace OrbOfDeception.Items
{
    public class OrbItemController : CollectibleItemController
    {
        private OrbItem OrbItem => item as OrbItem;
        
        protected override void Start()
        {
            base.Start();
            
            if (SaveSystem.IsOrbObtained(OrbItem))
                Destroy(gameObject);
        }

        protected override void AfterExitingItemObtainedMenu()
        {
            base.AfterExitingItemObtainedMenu();
            
            SaveSystem.currentOrbType = OrbItem.orbType;
            
            if (SaveSystem.currentOrbType == OrbController.OrbType.Pallid)
                GameManager.Orb.Appear();
            /*else if (SaveSystem.currentOrbType == OrbController.OrbType.Awakened)
                GameManager.Orb.ChangeColor();*/
            
            SaveSystem.AddOrbObtained(OrbItem.orbType);
        }
    }
}