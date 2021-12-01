using UnityEngine;

namespace OrbOfDeception.Core
{
    public class PauseController : MonoBehaviour
    {
        public bool IsPaused { get; private set; }

        private static PauseController _instance;
        public static PauseController Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(new GameObject()).AddComponent<PauseController>();
                }

                return _instance;
            }
        }

        public void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0;
        }
        
        public void Resume()
        {
            IsPaused = false;
            Time.timeScale = 1;
        }
    }
}