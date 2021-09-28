using UnityEngine;

namespace OrbOfDeception
{
    public class UIMaskController : MonoBehaviour
    {
        private Animator _animator;
        
        private static readonly int DisappearTrigger = Animator.StringToHash("Disappear");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Disappear()
        {
            _animator.SetTrigger(DisappearTrigger);
        }
    }
}
