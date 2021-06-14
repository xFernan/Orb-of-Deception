using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Player.Orb;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerGroupController : MonoBehaviour
    {

        public static PlayerGroupController Instance { get; private set; }
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private OrbController orbController;
        [SerializeField] private CameraController cameraController;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SetPositionInNewRoom(Vector3 newPlayerPosition)
        {
            var playerTransform = playerController.transform;
            
            var movement = newPlayerPosition - playerTransform.position;

            playerTransform.position = newPlayerPosition;
            orbController.transform.position += movement;
            cameraController.transform.position += movement;
        }
        
    }
}
