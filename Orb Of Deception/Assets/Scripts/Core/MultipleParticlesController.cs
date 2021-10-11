using UnityEngine;

namespace OrbOfDeception.Core
{
    public class MultipleParticlesController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particles;

        public void Play()
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }

        public void Play(Color particlesColor)
        {
            foreach (var particle in particles)
            {
                var main = particle.main;
                main.startColor = particlesColor;
                particle.Play();
            }
        }
        
        public void SetColor(Color newColor)
        {
            foreach (var particle in particles)
            {
                var main = particle.main;
                main.startColor = newColor;
            }
        }
    }
}
