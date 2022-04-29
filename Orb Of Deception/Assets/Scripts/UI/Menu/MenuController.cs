using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI.Menu
{
    public abstract class MenuController : MonoBehaviour
    {
        private Graphic[] _images;
        
        [HideInInspector] public bool isOpened;
        [HideInInspector] public bool canBeClosed = true;

        protected virtual void Awake()
        {
            _images = GetComponentsInChildren<Graphic>();
        }

        private void Start()
        {
            AllowRaycasts(false);
        }

        public virtual void Open()
        {
            isOpened = true;

            if (InGameMenuManager.Instance != null)
                InGameMenuManager.Instance.currentMenu = this;
            AllowRaycasts(true);
        }
        
        public virtual void Close()
        {
            isOpened = false;

            if (InGameMenuManager.Instance != null)
                InGameMenuManager.Instance.currentMenu = null;
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