using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrbOfDeception.Core.Scenes
{
    public class LevelChanger : MonoBehaviour
    {
        #region Variables

        private static LevelChanger _instance;
        public static LevelChanger Instance
        {
            get
            {
                if (_instance == null)
                {
                    var newLevelChanger = Instantiate((GameObject) Resources.Load("LevelChanger"));
                    _instance = newLevelChanger.GetComponent<LevelChanger>();
                }
                
                return _instance;
            }
        }
        
        public Animator anim;
        private string _sceneToLoad;
    
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");

        #endregion

        #region Métodos
        private void Awake()
        {
            anim = GetComponent<Animator>();
            
            DontDestroyOnLoad(this);

            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void FadeToScene(string sceneName)
        {
            _sceneToLoad = sceneName;
            anim.SetTrigger(FadeOut);
        }

        public void FadeToCurrentScene()
        {
            _sceneToLoad = SceneManager.GetActiveScene().name;
            anim.SetTrigger(FadeOut);
        }

        public void FadeToExitGame()
        {
            FadeToScene("Exit");
        }

        public void OnFadeComplete()
        {      
            if (_sceneToLoad.Equals("Exit"))
                Application.Quit();
            else
            {
                anim.ResetTrigger(FadeOut);
                SceneManager.LoadScene(_sceneToLoad);
            }
        }

        public void GoToScene(string sceneName)
        {
            anim.ResetTrigger(FadeOut);
            SceneManager.LoadScene(sceneName);
        }
        #endregion
    }
}
