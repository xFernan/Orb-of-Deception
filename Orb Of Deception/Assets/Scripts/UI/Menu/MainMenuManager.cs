using System.Collections;
using OrbOfDeception.Core.Scenes;
using UnityEngine;

namespace OrbOfDeception.UI.Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private MenuController mainMenuController;
        [SerializeField] private Animator titleAnimator;
        [SerializeField] private Texture2D cursorTexture;
        
        
        private static readonly int Appear = Animator.StringToHash("Appear");

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            StartCoroutine(ShowMainMenuCoroutine());
        }

        private IEnumerator ShowMainMenuCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            titleAnimator.SetTrigger(Appear);

            yield return new WaitForSeconds(1.5f);
            
            mainMenuController.Open();
        }
        
        public void QuitGame()
        {
            LevelChanger.Instance.FadeToExitGame();
        }

        public void StartGame()
        {
            LevelChanger.Instance.FadeToScene("Cinematic");
        }
    }
}