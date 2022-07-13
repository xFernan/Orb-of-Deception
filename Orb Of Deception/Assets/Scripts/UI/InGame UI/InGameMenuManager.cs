using System;
using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using OrbOfDeception.Core.Scenes;
using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        private static InGameMenuManager _instance;

        public static InGameMenuManager Instance => _instance;

        [SerializeField] private SoundsPlayer soundsPlayer;
        [SerializeField] private HideableElement backGroundController;
        [HideInInspector] public ItemObtainedMenu itemObtainedMenu;
        [HideInInspector] public TitleDisplayer titleDisplayer;
        [HideInInspector] public EndDemoMenuController endDemoMenuController;
        
        [SerializeField] private MenuController menuToOpenOnEscapePressed;

        [HideInInspector] public MenuController currentMenu;
        
        [HideInInspector] public bool isOpen;
        
        public Action onCloseMenu;

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            
            DontDestroyOnLoad(this);
            _instance = this;
            
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                foreach (Transform child2 in child)
                {
                    child2.gameObject.SetActive(true);
                }
            }
            
            backGroundController = GetComponentInChildren<PauseMenuDarkBackgroundController>();
            itemObtainedMenu = GetComponentInChildren<ItemObtainedMenu>();
            titleDisplayer = GetComponentInChildren<TitleDisplayer>();
            endDemoMenuController = GetComponentInChildren<EndDemoMenuController>();
        }

        private void Start()
        {
            GameManager.InputManager.Escape += OnEscapePressed;
            
            CursorController.Instance.DisplaySightCursor();
        }

        private void OnEscapePressed()
        {
            if (isOpen)
            {
                if (currentMenu.canBeClosed)
                    Close();
            }
            else
            {
                Open(menuToOpenOnEscapePressed);
            }
        }

        public void Open(MenuController menuToOpen, bool showDarkBackground = true, bool pauseGame = true)
        {
            if (pauseGame)
                PauseController.Instance.Pause();
            
            if (showDarkBackground)
                backGroundController.Show();
            
            menuToOpen.Open();

            soundsPlayer.Play("Open");
            
            MusicManager.Instance.ReduceVolume();
            
            CursorController.Instance.DisplayPointerCursor();
            
            isOpen = true;
        }
        
        public void Close()
        {
            PauseController.Instance.Resume();
            
            if (backGroundController.isShowed)
                backGroundController.Hide();
            
            currentMenu.Close();

            onCloseMenu?.Invoke();
            
            soundsPlayer.Play("Close");
            MusicManager.Instance.RevertReducedVolume();
            CursorController.Instance.DisplaySightCursor();
            
            isOpen = false;
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }

        public void QuitGame()
        {
            LevelChanger.Instance.FadeToExitGame();
        }
    }
}
