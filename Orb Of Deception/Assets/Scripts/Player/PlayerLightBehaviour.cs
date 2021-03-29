using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Nanref
{
    public class PlayerLightBehaviour : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float lightIntensity = 5;
        [SerializeField] private float lightIntensityVariation = 1;
        [SerializeField] private float lightIntensityVariationVelocity = 1;

        private Light2D _playerPointLight;
        #endregion
        
        #region Methods
        private void Awake()
        {
            _playerPointLight = GetComponent<Light2D>();
        }

        private void Update()
        {
            _playerPointLight.intensity = lightIntensity +
                                         Mathf.Sin(Time.time * lightIntensityVariationVelocity) *
                                         lightIntensityVariation;
        }
        #endregion
        
    }
}
