using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Gameplay.Orb;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class PlayerGroup : MonoBehaviour
    {
        private static PlayerGroup _instance;

        public static PlayerGroup Instance => _instance;
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

        public static PlayerController Player => Instance.playerController;
        public static OrbController Orb => Instance.orbController;
        public static CameraController Camera => Instance.cameraController;
        
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
