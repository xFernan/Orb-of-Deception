using UnityEngine;

namespace OrbOfDeception.Core
{
    public abstract class HideableElement : MonoBehaviour
    {
        public abstract void Show();
        public abstract void Hide();
        public virtual void Reset() {}
    }
}