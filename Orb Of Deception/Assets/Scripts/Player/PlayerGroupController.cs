using System;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Player;
using OrbOfDeception.Player.Orb;
using UnityEngine;

namespace OrbOfDeception
{
    public class PlayerGroupController : MonoBehaviour
    {

        public static PlayerGroupController instance;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private OrbController orbController;
        [SerializeField] private CameraController cameraController;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void SetPositionInNewRoom(Vector3 newPlayerPosition)
        {
            var movement = newPlayerPosition - playerController.transform.position;

            playerController.transform.position = newPlayerPosition;
            orbController.transform.position += movement;
            cameraController.transform.position += movement;
        }
        
    }
}
