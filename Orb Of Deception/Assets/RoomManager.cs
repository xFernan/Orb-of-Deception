using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private RoomChanger[] roomChangers;
        public static int targetRoomPlayerPositionID = -1;

        private void Awake()
        {
            if (targetRoomPlayerPositionID == -1)
            {
                return;
            }

            var playerGroup = PlayerGroupController.Instance;
            playerGroup.SetPositionInNewRoom(roomChangers[targetRoomPlayerPositionID - 1].GetIncomingPlayerPosition());
        }
    }
}
