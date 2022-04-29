using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Essence_of_Punishment;
using OrbOfDeception.UI;
using OrbOfDeception.UI.Menu;
using UnityEngine;

namespace OrbOfDeception.Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private int roomId;
        public string areaName = "Theater of the Origins";
        [SerializeField] private RoomChanger[] roomChangers;
        [SerializeField] private CameraLimits defaultCameraLimits;

        public static int targetRoomChangerID = -1;
        public static RoomManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            SetDefaultCameraLimits();
        }

        private void Start()
        {
            PlacePlayer();
        }

        private void PlacePlayer()
        {
            Vector3 positionToPlacePlayer;

            if (targetRoomChangerID != -1)
            {
                var roomChanger = roomChangers[targetRoomChangerID - 1];
                positionToPlacePlayer = roomChanger.GetPlayerPlacePosition();
                roomChanger.OnPlacingPlayerOnRoomChanger();
            }
            else
            {
                GameManager.Player.DeathController.ConfigRespawn();
                InGameMenuManager.Instance.titleDisplayer.DisplayTitle(roomId == 1 ? "Backstage" : "Theater of the Origins"); // Provisional
                var spawnPositionSaved = SaveSystem.GetSpawnPosition();

                if (spawnPositionSaved != default)
                {
                    positionToPlacePlayer = spawnPositionSaved;
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
            }

            GameManager.Player.isControlled = true;
            GameManager.Instance.SetPositionInNewRoom(positionToPlacePlayer);
        }

        public int GetRoomID()
        {
            return roomId;
        }

        public void SetDefaultCameraLimits()
        {
            GameManager.Camera.ChangeCameraLimits(defaultCameraLimits);
        }

        public void CollectRemainingEssences()
        {
            var essences = FindObjectsOfType<EssenceOfPunishmentController>();

            foreach (var essence in essences)
            {
                essence.CollectOnRemainingIfHasNotBeenCollected();
            }
        }
        
    }
}
