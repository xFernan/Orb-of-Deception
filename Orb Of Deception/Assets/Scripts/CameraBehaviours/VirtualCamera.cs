using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class VirtualCamera : MonoBehaviour
    {
        private static VirtualCamera _instance;
        public static VirtualCamera Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<VirtualCamera>();
                    if (_instance == null)
                    {
                        var newMusicManager = Instantiate((GameObject) Resources.Load("VirtualCamera"));
                        _instance = newMusicManager.GetComponent<VirtualCamera>();
                    }
                }
                
                return _instance;
            }
        }
        
        public Camera CameraComponent { get; private set; }
        
        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);

            CameraComponent = GetComponent<Camera>();
            
            DontDestroyOnLoad(this);
        }
    }
}
