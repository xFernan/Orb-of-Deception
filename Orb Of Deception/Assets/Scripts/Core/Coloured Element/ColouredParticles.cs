using UnityEngine;

namespace OrbOfDeception.Core
{
    public class ColouredParticles : ColouredElement
    {
        private ParticleSystem _particles;

        protected override void Awake()
        {
            _particles = GetComponent<ParticleSystem>();
            base.Awake();
        }

        protected override void SetToBlack()
        {
            var main = _particles.main;
            main.startColor = blackColor;
        }

        protected override void SetToWhite()
        {
            var main = _particles.main;
            main.startColor = whiteColor;
        }

        protected override void SetToOtherColor()
        {
            var main = _particles.main;
            main.startColor = otherColor;
        }
    }
}
