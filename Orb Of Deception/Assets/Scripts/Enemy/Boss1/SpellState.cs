using System.Collections;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class SpellState : EnemyState
    {
        #region Variables

        private readonly Boss1Parameters _parameters;
        private readonly Boss1SpellCaster _spellCaster;
        private readonly MonoBehaviour _monoBehaviour;

        private Coroutine _exitSpellStateCoroutine;
        
        #endregion
            
        #region Methods
        
        public SpellState(Boss1Controller enemy) : base(enemy)
        {
            _parameters = enemy.Parameters;
            _spellCaster = enemy.spellCaster;
            _monoBehaviour = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            
            _exitSpellStateCoroutine = _monoBehaviour.StartCoroutine(ExitSpellStateCoroutine());
        }

        public override void Exit()
        {
            base.Exit();
            
            _monoBehaviour.StopCoroutine(_exitSpellStateCoroutine);
        }

        private IEnumerator ExitSpellStateCoroutine()
        {
            yield return new WaitForSeconds(_parameters.Stats.castSpellDelay);
            
            _spellCaster.CastSpell();
            
            yield return new WaitForSeconds(_parameters.Stats.exitStateDelay);
            
            enemy.SetState(Boss1Controller.TeleportingState);
        }
        
        #endregion
    }
}