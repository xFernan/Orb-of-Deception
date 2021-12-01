using System;
using OrbOfDeception.Collectible;
using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace OrbOfDeception.UI
{
    public class MenuManager : MonoBehaviour
    {
        private static MenuManager _instance;

        public static MenuManager Instance => _instance;
        
        [SerializeField] private HideableElement backGroundController;
        [SerializeField] private SubMenuController menuToOpenOnEscapePressed;

        [HideInInspector] public SubMenuController currentMenu;
        
        [HideInInspector] public bool isOpen;

        [HideInInspector] public Action onCloseMenu;

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            
            DontDestroyOnLoad(this);
            _instance = this;
            
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            
            backGroundController = GetComponentInChildren<PauseMenuDarkBackgroundController>();
        }

        private void Start()
        {
            GameManager.InputManager.Escape += OnEscapePressed;
        }

        private void OnEscapePressed()
        {
            if (isOpen) Close();
            else Open(menuToOpenOnEscapePressed);
        }

        public void Open(SubMenuController menuToOpen)
        {
            PauseController.Instance.Pause();
            
            backGroundController.Show();
            
            menuToOpen.Open();

            isOpen = true;
        }
        
        public void Close()
        {
            PauseController.Instance.Resume();
            
            backGroundController.Hide();
            
            currentMenu.Close();

            onCloseMenu?.Invoke();
            
            isOpen = false;
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }
        
    }
}
