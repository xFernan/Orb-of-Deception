using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI.Health_Bar
{
    public class UIHeartController : MonoBehaviour
    {
        private bool _isBroken = false;
        
        private Animator _animator;
        
        private static readonly int DisappearTrigger = Animator.StringToHash("Disappear");
        private static readonly int RecoverTrigger = Animator.StringToHash("Recover");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Break()
        {
            _isBroken = true;
            _animator.SetTrigger(DisappearTrigger);
        }

        public void Recover()
        {
            _isBroken = true;
            _animator.SetTrigger(RecoverTrigger);
        }

        public bool IsBroken()
        {
            return _isBroken;
        }
    }
}
