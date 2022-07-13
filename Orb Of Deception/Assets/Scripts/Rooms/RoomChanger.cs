using System;
using OrbOfDeception.Audio;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core.Scenes;
using OrbOfDeception.Player;
using OrbOfDeception.UI;
using OrbOfDeception.UI.InGame_UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace OrbOfDeception.Rooms
{
    public class RoomChanger : MonoBehaviour, IPlayerHittable
    {
        [SerializeField] private Transform playerPositionReference;
        [SerializeField] private string sceneName;
        [SerializeField] private int nextRoomPlayerPositionID;
        [SerializeField] private UnityEvent onEnterEvent;

        [Space]
        
        [SerializeField] private bool showAreaTitle = false;
        [SerializeField] private CameraLimits onEnterCameraLimits;
        
        [Space]
        
        [SerializeField] private bool playMusicOnEnter = false;
        [ShowIf(nameof(playMusicOnEnter), true)] [SerializeField] private string musicNameOnEnter;

        public void OnPlacingPlayerOnRoomChanger()
        {
            GameManager.Camera.SetNewCameraLimits(onEnterCameraLimits);
            //onEnterEvent.Invoke();
            
            if (showAreaTitle)
                InGameMenuManager.Instance.titleDisplayer.DisplayTitle(RoomManager.CurrentRoom.areaName);

            if (playMusicOnEnter)
            {
                if (musicNameOnEnter == default)
                    MusicManager.Instance.StopMusic();
                else
                    MusicManager.Instance.PlayMusic(musicNameOnEnter, 0.15f);
                    
            }
        }

        public Vector3 GetPlayerPlacePosition()
        {
            return playerPositionReference.position;
        }

        public void OnPlayerHitEnter()
        {
            ChangeScreen();
            onEnterEvent.Invoke();
        }

        public void OnPlayerHitExit()
        {
            
        }

        private void ChangeScreen()
        {
            RoomManager.CurrentRoom.OnExitRoom();
            LevelChanger.Instance.FadeToScene(sceneName);
            RoomManager.targetRoomChangerID = nextRoomPlayerPositionID;
            GameManager.Player.isControlled = false;
            
            if (playMusicOnEnter)
                MusicManager.Instance.StopMusic();
        }
    }
}
