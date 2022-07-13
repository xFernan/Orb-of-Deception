using OrbOfDeception.UI.InGame_UI;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI
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

        public virtual void Open()
        {
            isOpened = true;

            if (InGameMenuManager.Instance != null)
                InGameMenuManager.Instance.currentMenu = this;
        }
        
        public virtual void Close()
        {
            isOpened = false;

            if (InGameMenuManager.Instance != null)
                InGameMenuManager.Instance.currentMenu = null;
        }
    }
}