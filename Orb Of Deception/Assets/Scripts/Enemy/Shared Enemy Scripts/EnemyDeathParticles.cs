using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyDeathParticles : MonoBehaviour
    {
        private MultipleParticlesController _particles;
        
        private void Awake()
        {
            _particles = GetComponent<MultipleParticlesController>();
        }

        public void PlayParticles()
        {
            transform.parent = null;
            _particles.Play();
        }
    }
}
