using UnityEngine;

namespace OrbOfDeception.UI
{
    public class UIArrowSelector : MonoBehaviour
    {
        private bool _isVisible;

        private Animator _animator;
        
        private static readonly int IsVisible = Animator.StringToHash("IsVisible");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(IsVisible, _isVisible);
        }

        public void Show()
        {
            _isVisible = true;
        }

        public void Hide()
        {
            _isVisible = false;
        }
    }
}
