using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI
{
    public abstract class SubMenuController : MonoBehaviour
    {
        private Graphic[] _images;
        private MenuManager _menuManager;
        
        [HideInInspector] public bool isOpened;

        protected virtual void Awake()
        {
            _images = GetComponentsInChildren<Graphic>();
            _menuManager = GetComponentInParent<MenuManager>();
        }

        private void Start()
        {
            AllowRaycasts(false);
        }

        public virtual void Open()
        {
            isOpened = true;

            _menuManager.currentMenu = this;
            AllowRaycasts(true);
        }
        
        public virtual void Close()
        {
            isOpened = false;

            _menuManager.currentMenu = null;
            AllowRaycasts(false);
        }

        private void AllowRaycasts(bool isAllowed)
        {
            foreach (var image in _images)
            {
                image.raycastTarget = isAllowed;
            }
        }
    }
}