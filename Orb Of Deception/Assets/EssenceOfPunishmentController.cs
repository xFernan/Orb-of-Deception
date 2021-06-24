using System;
using UnityEngine;

namespace OrbOfDeception
{
    public class EssenceOfPunishmentController : MonoBehaviour
    {

        [SerializeField] private GameObject onAcquireParticles;
        [SerializeField] private ParticleSystem particlesTrail;
        [SerializeField] private ParticleSystem particlesIdle;

        private Animator _animator;
        private static readonly int Acquire = Animator.StringToHash("Disappear");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnAcquire()
        {
            // Provisional:
            Instantiate(onAcquireParticles, transform.position, Quaternion.identity);
            particlesIdle.Stop();
            particlesTrail.Stop();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player" /*Provisional*/))
            {
                _animator.SetTrigger(Acquire);
            }
        }
    }
}
