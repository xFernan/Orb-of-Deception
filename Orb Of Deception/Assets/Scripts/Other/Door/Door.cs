using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using OrbOfDeception.Rooms;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OrbOfDeception.Door
{
    public abstract class Door : MonoBehaviour, IOtherTriggerEnter
    {
        [SerializeField] private bool dependsOnSwitch = false;
        [ShowIf("dependsOnSwitch")] [SerializeField] private int doorID;
        
        private Animator _animator;
        protected SoundsPlayer soundsPlayer;
        
        private static readonly int OpenTrigger = Animator.StringToHash("Open");
        private static readonly int OpenedTrigger = Animator.StringToHash("Opened");
        private static readonly int CloseTrigger = Animator.StringToHash("Close");
        private static readonly int ClosedTrigger = Animator.StringToHash("Closed");

        private bool _isOpened;

        private void Awake()
        {
            _isOpened = true;
            
            _animator = GetComponent<Animator>();
            soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        private void Start()
        {
            if (dependsOnSwitch)
            {
                _isOpened = SaveSystem.IsDoorOpened(doorID);
                if (!_isOpened)
                    _animator.SetTrigger(ClosedTrigger);
            }
        }

        public void Open()
        {
            if (_isOpened)
                return;

            if (dependsOnSwitch)
                SaveSystem.AddDoorOpened(doorID);
            
            _isOpened = true;
            _animator.SetTrigger(OpenTrigger);

            OnOpening();
        }

        protected virtual void OnOpening()
        {
            
        }
        
        public void Close()
        {
            if (!_isOpened)
                return;
            
            _isOpened = false;
            _animator.SetTrigger(CloseTrigger);

            OnClosing();
        }

        protected virtual void OnClosing()
        {
            
        }
        
        public void Response()
        {
            if (_isOpened)
                Close();
            else
                Open();
        }
        
        protected virtual void OnClosingEnd() {}
        
        protected virtual void OnOpeningEnd() {}

        public void OnOtherTriggerEnter()
        {
            Open();
        }
    }
}