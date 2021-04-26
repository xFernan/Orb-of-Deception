using OrbOfDeception.Core.Input;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerAnimationController
    {
        private readonly Animator _animator;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputManager _inputManager;
        private readonly PlayerGroundDetector _playerGroundDetector;
        
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsOnTheGround = Animator.StringToHash("IsOnTheGround");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");

        public PlayerAnimationController(Animator animator, Rigidbody2D rigidbody, InputManager inputManager, PlayerGroundDetector playerGroundDetector)
        {
            _animator = animator;
            _rigidbody = rigidbody;
            _inputManager = inputManager;
            _playerGroundDetector = playerGroundDetector;
        }

        public void Update()
        {
            _animator.SetBool(IsMoving, _inputManager.GetHorizontal() != 0); // Cambiar
            _animator.SetBool(IsOnTheGround, _playerGroundDetector.IsOnTheGround());
            _animator.SetBool(IsFalling, _rigidbody.velocity.y < 0);
        }
    }
}