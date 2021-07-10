using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Gameplay.Orb;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class PlayerGroupController : MonoBehaviour
    {
        private static PlayerGroupController _instance;
        public static PlayerGroupController Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<PlayerGroupController>();
                
                if (_instance != null) return _instance;

                _instance = Instantiate(Resources.Load("PlayerGroup") as GameObject)
                    .GetComponent<PlayerGroupController>();
                
                return _instance;
            }
        }

        public PlayerController playerController;
        public OrbController orbController;
        public CameraController cameraController;
        
        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            
            DontDestroyOnLoad(this);
            _instance = this;
        }
        
        private void OnDestroy()
        {
            _instance = null;
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
