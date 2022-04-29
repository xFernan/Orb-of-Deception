using UnityEngine;

namespace OrbOfDeception.Core
{
    public abstract class HideableElement : MonoBehaviour
    {
        [HideInInspector] public bool isShowed;

        public virtual void Show()
        {
            isShowed = true;
        }

        public virtual void Hide()
        {
            isShowed = false;
        }
        
        public virtual void Reset() {}
    }
}