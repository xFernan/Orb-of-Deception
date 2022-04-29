using System;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core.Scenes;
using OrbOfDeception.Player;
using OrbOfDeception.UI;
using OrbOfDeception.UI.Menu;
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

        public void OnPlacingPlayerOnRoomChanger()
        {
            GameManager.Camera.ChangeCameraLimits(onEnterCameraLimits);
            onEnterEvent.Invoke();
            
            if (!showAreaTitle) return;
            InGameMenuManager.Instance.titleDisplayer.DisplayTitle(RoomManager.Instance.areaName);
        }

        public Vector3 GetPlayerPlacePosition()
        {
            return playerPositionReference.position;
        }

        public void OnPlayerHitEnter()
        {
            ChangeScreen();
        }

        public void OnPlayerHitExit()
        {
            
        }

        private void ChangeScreen()
        {
            RoomManager.Instance.CollectRemainingEssences();
            LevelChanger.Instance.FadeToScene(sceneName);
            RoomManager.targetRoomChangerID = nextRoomPlayerPositionID;
            GameManager.Player.isControlled = false;
        }
    }
}
