using System;
using System.Collections;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    public class Enemy3StaffController : MonoBehaviour
    {
        [SerializeField] private GameObject spellPrefab;
        [SerializeField] private Transform spellSpawnTransform;
        [SerializeField] private ParticleSystem idleParticles;
        [SerializeField] private ParticleSystem attackParticles;

        private Coroutine _attackDelayCoroutine;
        
        private Enemy3Parameters _enemyParameters;
        private Animator _animator;
        
        private static readonly int IdleTrigger = Animator.StringToHash("Idle");
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");

        private void Awake()
        {
            _enemyParameters = GetComponentInParent<Enemy3Parameters>();
            _animator = GetComponent<Animator>();
        }

        public void EnterAttack()
        {
            PrepareAttack();
        }

        private void ExitAttack()
        {
            StopCoroutine(_attackDelayCoroutine);
            idleParticles.Stop();
        }
        
        private void PrepareAttack()
        {
            idleParticles.Play();
            _attackDelayCoroutine = StartCoroutine(AttackCoroutine());
        }

        private void Attack()
        {
            _animator.ResetTrigger(IdleTrigger);
            _animator.SetTrigger(AttackTrigger);
        }
        
        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(_enemyParameters.timePreparingAttack);
            Attack();
            
            yield return new WaitForSeconds(_enemyParameters.timeBetweenAttacks);
            PrepareAttack();
        }
        
        private void InvokeAttack()
        {
            idleParticles.Stop();
            attackParticles.Play();
            Instantiate(spellPrefab, spellSpawnTransform.position, Quaternion.identity);
        }

        public void UpdateXOffset(bool isOrientationRight)
        {
            var localPosition = transform.localPosition;
            localPosition.x = Mathf.Abs(localPosition.x) * (isOrientationRight ? -1 : 1);
            transform.localPosition = localPosition;
        }

        public void StopAttack()
        {
            ExitAttack();
            _animator.SetTrigger(IdleTrigger);
        }
    }
}
