using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class AnimationController
    {
        private readonly Animator _animator;
        private readonly Rigidbody2D _rigidbody;
        private readonly GroundDetector _groundDetector;
        
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsOnTheGround = Animator.StringToHash("IsOnTheGround");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int IsKneeling = Animator.StringToHash("IsKneeling");

        public AnimationController(Animator animator, Rigidbody2D rigidbody, GroundDetector groundDetector)
        {
            _animator = animator;
            _rigidbody = rigidbody;
            _groundDetector = groundDetector;
        }

        public void Update()
        {
            _animator.SetBool(IsMoving, GameManager.Player.HorizontalMovementController.IsMoving);
            _animator.SetBool(IsOnTheGround, _groundDetector.IsOnTheGround());
            _animator.SetBool(IsFalling, _rigidbody.velocity.y < 0);
            _animator.SetBool(IsKneeling, GameManager.Player.KneelController.isKneeling);
        }
    }
}