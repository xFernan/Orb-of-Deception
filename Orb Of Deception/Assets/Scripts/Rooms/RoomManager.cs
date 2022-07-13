using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrbOfDeception.Audio;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Essence_of_Punishment;
using OrbOfDeception.UI;
using OrbOfDeception.UI.InGame_UI;
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
        public static RoomManager CurrentRoom { get; private set; }
        private List<LoopSoundPlayer> _loopSounds;

        private void Awake()
        {
            CurrentRoom = this;
            _loopSounds = FindObjectsOfType<LoopSoundPlayer>().ToList();
            
            GameManager.Camera.SetNewCameraLimits(defaultCameraLimits);
        }

        private void Start()
        {
            StartCoroutine(PlacePlayerCoroutine());
        }

        private IEnumerator PlacePlayerCoroutine()
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
                        yield break;
                    }
                }
            }
            
            GameManager.Instance.SetPositionInNewRoom(positionToPlacePlayer);

            yield return 0;
            
            GameManager.Player.isControlled = true;
        }

        public int GetRoomID()
        {
            return roomId;
        }

        public void SetDefaultCameraLimits()
        {
            GameManager.Camera.LerpToNewCameraLimits(defaultCameraLimits);
        }

        public void OnExitRoom()
        {
            // Collect remaining essences
            var essences = FindObjectsOfType<EssenceOfPunishmentController>();

            foreach (var essence in essences)
            {
                essence.CollectOnRemainingIfHasNotBeenCollected();
            }
            
            // Stop all loop sounds
            foreach (var loopSound in _loopSounds)
            {
                if (loopSound.stopOnSceneExit)
                    loopSound.Stop();
            }
        }
        
    }
}
