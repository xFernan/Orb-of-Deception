using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class HurtController
    {
        private readonly float _timeInvulnerable;
        private readonly Animator _spriteAnimator;
        
        private readonly MethodDelayer _invulnerableEndDelay;
        
        private bool _isInvulnerable;
        
        
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Normal = Animator.StringToHash("Normal");

        public HurtController(float timeInvulnerable, Animator spriteAnimator)
        {
            _timeInvulnerable = timeInvulnerable;
            _spriteAnimator = spriteAnimator;
            
            _isInvulnerable = false;
            _invulnerableEndDelay = new MethodDelayer(EndHurt);
        }

        public void Update(float deltaTime)
        {
            _invulnerableEndDelay.Update(deltaTime);
        }
        
        public void StartHurt()
        {
            if (_isInvulnerable) return;
            
            _isInvulnerable = true;
            _spriteAnimator!.SetTrigger(Hurt);
            _invulnerableEndDelay.SetNewDelay(_timeInvulnerable);
        }

        private void EndHurt()
        {
            _isInvulnerable = false;
            _spriteAnimator!.SetTrigger(Normal);
        }

        public bool IsInvulnerable()
        {
            return _isInvulnerable;
        }
    }
}