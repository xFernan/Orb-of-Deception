using UnityEngine;

namespace OrbOfDeception
{
    public abstract class Door : MonoBehaviour, IOtherTriggerEnter
    {
        private Animator _animator;
        
        private static readonly int OpenTrigger = Animator.StringToHash("Open");
        private static readonly int OpenedTrigger = Animator.StringToHash("Opened");
        private static readonly int CloseTrigger = Animator.StringToHash("Close");
        private static readonly int ClosedTrigger = Animator.StringToHash("Closed");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public virtual void Open()
        {
            _animator.SetTrigger(OpenTrigger);
        }

        public virtual void Close()
        {
            _animator.SetTrigger(CloseTrigger);
        }
        protected virtual void OnClosingEnd() {}
        
        protected virtual void OnOpeningEnd() {}

        public void OnOtherTriggerEnter()
        {
            Open();
        }
    }
}