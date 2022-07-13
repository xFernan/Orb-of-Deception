using OrbOfDeception.Orb;
using OrbOfDeception.Player;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception
{
    public class RoomChangerDebug : MonoBehaviour
    {
        public void UnlockAll()
        {
            GameManager.Orb.Appear();
            SaveSystem.currentOrbType = OrbController.OrbType.Awakened;
            SaveSystem.UnlockMask(PlayerMaskController.MaskType.VigorousMask);
            SaveSystem.UnlockMask(PlayerMaskController.MaskType.ScarletMask);
        }
    }
}
