using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class Boss1SpellCaster : MonoBehaviour
    {
        [SerializeField] private ParticleSystem spellParticles;
        [SerializeField] private MultipleParticlesController attackParticles;
        [SerializeField] private GameObject spellPrefab;
        [SerializeField] private Transform leftCorner;
        [SerializeField] private Transform rightCorner;
        
        private Animator _animator;
        private Boss1Parameters _parameters;
        private SoundsPlayer _soundsPlayer;

        private bool _canAttack = true;
        
        private static readonly int CastSpellTrigger = Animator.StringToHash("CastSpell");
        private static readonly int IdleTrigger = Animator.StringToHash("Idle");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _parameters = GetComponentInParent<Boss1Parameters>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        public void CastSpell()
        {
            spellParticles.Play();
            _animator.SetTrigger(CastSpellTrigger);
        }

        private void Attack()
        {
            if (!_canAttack)
                return;
            
            spellParticles.Stop();
            attackParticles.Play();

            _soundsPlayer.Play("Spell");
            
            var originPosition = transform.position;
            var firstSpellVectorDirection = (leftCorner.position - originPosition).normalized;
            var lastSpellVectorDirection = (rightCorner.position - originPosition).normalized;
            
            for (var i = 0; i < _parameters.Stats.charmAmount; i++)
            {
                var spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
                var spellDirection = Vector3.Lerp(firstSpellVectorDirection, lastSpellVectorDirection,
                    i / ((float) _parameters.Stats.charmAmount - 1)).normalized;
                spell.GetComponent<Boss1SpellController>().SetSpellDirection(spellDirection);
            }
        }

        public void StopAttack()
        {
            spellParticles.Stop();
            _animator.SetTrigger(IdleTrigger);
            _canAttack = false;
        }
    }
}
