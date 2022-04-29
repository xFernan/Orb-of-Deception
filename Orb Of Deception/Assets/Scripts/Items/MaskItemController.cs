using OrbOfDeception.Rooms;

namespace OrbOfDeception.Items
{
    public class MaskItemController : CollectibleItemController
    {
        private MaskItem MaskItem => item as MaskItem;

        protected override void Start()
        {
            base.Start();

            if (SaveSystem.IsMaskUnlocked(MaskItem))
                gameObject.SetActive(false);
        }

        protected override void AfterExitingItemObtainedMenu()
        {
            base.AfterExitingItemObtainedMenu();
            
            SaveSystem.UnlockMask(MaskItem.maskType);
        }
    }
}