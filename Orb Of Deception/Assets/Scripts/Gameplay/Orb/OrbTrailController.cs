using UnityEngine;

namespace OrbOfDeception.Orb
{
    public class OrbTrailController
    {
        private readonly TrailRenderer _orbTrail;
        
        public OrbTrailController(TrailRenderer orbTrail)
        {
            _orbTrail = orbTrail;
        }
        
        public void SetTrailColor(Color trailColor)
        {
            var gradient = new Gradient();

            var colorKey = new GradientColorKey[2];
            colorKey[0].color = trailColor;
            colorKey[0].time = 0.0f;

            var alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
            
            _orbTrail.colorGradient = gradient;
        }
    }
}