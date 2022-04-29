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
            GameManager.Player.isControlled = false;
            IsPaused = true;
            Time.timeScale = 0;
        }
        
        public void Resume()
        {
            GameManager.Player.isControlled = true;
            IsPaused = false;
            Time.timeScale = 1;
        }
    }
}