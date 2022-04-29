using System;
using UnityEngine;


namespace OrbOfDeception.Core
{
    public class LightTwinkleBehaviour : MonoBehaviour
    {
        private enum TwinkleType
        {
            InnerRadius,
            OuterRadius,
            Intensity
        }

        [SerializeField] private TwinkleType twinkleType;
        [SerializeField] private float twinkleBaseValue;
        [SerializeField] private float twinkleVariation;
        [SerializeField] private float twinkleVelocity;
        
        private UnityEngine.Rendering.Universal.Light2D _light;

        private void Awake()
        {
            _light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }

        private void Update()
        {
            var newTwinkleValue = Mathf.Sin(Time.time * twinkleVelocity) * twinkleVariation + twinkleBaseValue;
            switch (twinkleType)
            {
                case TwinkleType.InnerRadius:
                    _light.pointLightInnerRadius = newTwinkleValue;
                    break;
                case TwinkleType.OuterRadius:
                    _light.pointLightOuterRadius = newTwinkleValue;
                    break;
                case TwinkleType.Intensity:
                    _light.intensity = newTwinkleValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
