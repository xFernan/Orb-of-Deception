using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace OrbOfDeception
{
    public class LightTwinkleBehaviour : MonoBehaviour
    {

        [SerializeField] private float twinkleIntensityVariation;
        [SerializeField] private float twinkleVelocity;
        
        private Light2D _light;
        private float _baseIntensity;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _baseIntensity = _light.pointLightInnerRadius;
        }

        private void Update()
        {
            _light.pointLightInnerRadius = Mathf.Sin(Time.time * twinkleVelocity) * twinkleIntensityVariation + _baseIntensity;
        }
    }
}
