using OrbOfDeception.Core;
using OrbOfDeception.Orb;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Rooms
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
