using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerAnimationController
    {
        private readonly Animator _animator;
        private readonly Rigidbody2D _rigidbody;
        private readonly GroundDetector _groundDetector;
        
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsOnTheGround = Animator.StringToHash("IsOnTheGround");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int IsKneeling = Animator.StringToHash("IsKneeling");
        private static readonly int IsDying = Animator.StringToHash("IsDying");

        public PlayerAnimationController(Animator animator, Rigidbody2D rigidbody, GroundDetector groundDetector)
        {
            _animator = animator;
            _rigidbody = rigidbody;
            _groundDetector = groundDetector;
        }

        public void Update()
        {
            _animator.SetBool(IsDying, GameManager.Player.DeathController.isDying);
            if (PauseController.Instance.IsPaused)
                return;
            _animator.SetBool(IsMoving, GameManager.Player.HorizontalMovementController.IsMoving);
            _animator.SetBool(IsOnTheGround, _groundDetector.IsOnTheGround());
            _animator.SetBool(IsFalling, _rigidbody.velocity.y < 0);
            _animator.SetBool(IsKneeling, GameManager.Player.KneelController.isKneeling);
        }

        public void SetAnimatorUpdateToUnscaledTime()
        {
            var player = GameManager.Player;
            
            player.SpriteAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            var orb = GameManager.Orb;
            orb.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            orb.colorChangeParticles.SetToUnscaledTime(true);
        }
        
        public void SetAnimatorUpdateToNormal()
        {
            var player = GameManager.Player;
            
            player.SpriteAnimator.updateMode = AnimatorUpdateMode.Normal;
            
            _animator.updateMode = AnimatorUpdateMode.Normal;

            var orb = GameManager.Orb;
            orb.Animator.updateMode = AnimatorUpdateMode.Normal;
            
            orb.colorChangeParticles.SetToUnscaledTime(false);
        }
    }
}