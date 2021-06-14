using System;
using System.Collections;
using System.Collections.Generic;
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
            _baseIntensity = _light.pointLightOuterRadius;
        }

        private void Update()
        {
            _light.pointLightOuterRadius = Mathf.Sin(Time.time * twinkleVelocity) * twinkleIntensityVariation + _baseIntensity;
        }
    }
}
