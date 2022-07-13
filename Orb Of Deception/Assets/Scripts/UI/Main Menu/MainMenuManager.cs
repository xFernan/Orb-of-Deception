using System;
using System.Collections;
using OrbOfDeception.Audio;
using OrbOfDeception.Core.Scenes;
using UnityEngine;

namespace OrbOfDeception.UI.Main_Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Camera mainMenuCamera;
        [SerializeField] private SoundsPlayer soundsPlayer;
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
            StartCoroutine(ShowMainMenuCoroutine());
            
            CursorController.Instance.DisplayPointerCursor();
            
            MusicManager.Instance.PlayMusic("MainMenu");
        }

        private IEnumerator ShowMainMenuCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            titleAnimator.SetTrigger(Appear);

            yield return new WaitForSeconds(1.5f);
            
            mainMenuController.Open();
            soundsPlayer.Play("Open");
        }
        
        public void QuitGame()
        {
            LevelChanger.Instance.FadeToExitGame();
            MusicManager.Instance.StopMusic();
        }

        public void StartGame()
        {
            LevelChanger.Instance.FadeToScene("Cinematic");
            MusicManager.Instance.StopMusic();
        }
    }
}