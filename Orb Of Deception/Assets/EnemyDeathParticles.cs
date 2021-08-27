using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrbOfDeception
{
    public class EnemyDeathParticles : MonoBehaviour
    {
        private ParticleSystem _spawnParticles;
        
        private void Awake()
        {
            _spawnParticles = GetComponentInChildren<ParticleSystem>();
        }

        public void PlayParticles()
        {
            transform.parent = null;
            _spawnParticles.Play();
        }
    }
}