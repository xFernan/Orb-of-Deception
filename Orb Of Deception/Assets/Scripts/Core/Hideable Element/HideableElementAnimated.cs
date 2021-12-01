using UnityEngine;

namespace OrbOfDeception.Core
{
    public class HideableElementAnimated : HideableElement
    {
        private Animator _animator;
        
        private static readonly int IsVisible = Animator.StringToHash("IsVisible");

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Show()
        {
            _animator.SetBool(IsVisible, true);
        }

        public override void Hide()
        {
            _animator.SetBool(IsVisible, false);
        }
    }
}
