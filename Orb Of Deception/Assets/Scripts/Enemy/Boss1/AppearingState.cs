using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class AppearingState : EnemyState
    {
        #region Variables

        private readonly Animator _spriteAnimator;
        private readonly ParticleSystem _idleParticles;
        private readonly LightController _lightController;
        
        private static readonly int Appear = Animator.StringToHash("Appear");

        #endregion
            
        #region Methods
        
        public AppearingState(Boss1Controller enemy) : base(enemy)
        {
            _spriteAnimator = enemy.spriteAnim;
            _idleParticles = enemy.idleParticles;
            _lightController = enemy.lightController;
        }

        public override void Enter()
        {
            base.Enter();
            
            _spriteAnimator.SetTrigger(Appear);
            _idleParticles.Play();
            _lightController.Appear();
        }
        
        #endregion
    }
}