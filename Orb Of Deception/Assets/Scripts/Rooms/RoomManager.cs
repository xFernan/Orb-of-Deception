using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception.Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private RoomChanger[] roomChangers;
        [HideInInspector] public CameraLimits cameraLimits;
        
        public static int targetRoomChangerID = -1;
        public static RoomManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            cameraLimits = GetComponentInChildren<CameraLimits>();
            GameManager.Camera.cameraLimits = cameraLimits;

            PlacePlayer();
        }

        private void PlacePlayer()
        {
            Vector3 positionToPlacePlayer;
            
            if (targetRoomChangerID != -1)
            {
                positionToPlacePlayer = roomChangers[targetRoomChangerID - 1].GetPlayerPlacePosition();
            }
            else
            {
                var spawnDebug = GameObject.FindGameObjectWithTag("SpawnDebug");
                if (spawnDebug != null)
                {
                    positionToPlacePlayer = spawnDebug.transform.position;
                }
                else
                {
                    return;
                }
            }

            GameManager.Instance.SetPositionInNewRoom(positionToPlacePlayer);
        }
    }
}
