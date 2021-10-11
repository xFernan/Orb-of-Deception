using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class OrbHitParticlesController : MonoBehaviour
    {
        private ParticleSystem particles;
        private Transform hitEntityTransform;
        private Vector3 _localPosition;
        
        private void Awake()
        {
            particles = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (hitEntityTransform != null)
            {
                transform.position = hitEntityTransform.position + _localPosition;
            }
        }

        public void Play(GameObject hitEntity)
        {
            var main = particles.main;
            main.startColor = GameManager.Orb.CurrentParticlesColor;
            particles.Play();

            hitEntityTransform = hitEntity.transform;

            _localPosition = transform.position - hitEntityTransform.position;
        }
    }
}
