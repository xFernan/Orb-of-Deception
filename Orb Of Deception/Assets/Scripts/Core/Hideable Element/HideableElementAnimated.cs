using UnityEngine;

namespace OrbOfDeception.Core
{
    public class HideableElementAnimated : HideableElement
    {
        private Animator _animator;
        
        private static readonly int IsVisible = Animator.StringToHash("IsVisible");
        private static readonly int HiddenTrigger = Animator.StringToHash("HiddenTrigger");

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Show()
        {
            base.Show();
            
            _animator.SetBool(IsVisible, true);
        }

        public override void Hide()
        {
            base.Hide();
            
            _animator.SetBool(IsVisible, false);
        }

        public void SetHidden()
        {
            isShowed = false;
            
            _animator.SetBool(IsVisible, false);
            _animator.SetTrigger(HiddenTrigger);
        }
    }
}
