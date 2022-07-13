using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core.Input;
using OrbOfDeception.Orb;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance => _instance;
        /*{
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<PlayerGroup>();
                
                if (_instance != null) return _instance;

                _instance = Instantiate(Resources.Load("PlayerGroup") as GameObject)
                    .GetComponent<PlayerGroup>();
                Debug.Log("Instantiating PG");
                
                return _instance;
            }
        }*/

        public PlayerController playerController;
        public OrbController orbController;
        public CameraController cameraController;
        public InputManager inputManager;

        public static PlayerController Player => Instance.playerController;
        public static OrbController Orb => Instance.orbController;
        public static CameraController Camera => Instance.cameraController;
        public static InputManager InputManager => Instance.inputManager;

        public const int Ppu = 16;
        public const int WidthInPixels = 320;
        public const int HeightInPixels = 180;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Debug.Log("Destroying Game Manager");
                Destroy(gameObject);
            }
            
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
            orbController.Reposition(movement);
            cameraController.RePosition();
            //cameraController.transform.position += movement;
        }
    }
}
