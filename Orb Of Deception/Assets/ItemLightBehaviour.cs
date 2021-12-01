using System;
using DG.Tweening;
using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace OrbOfDeception
{
    public class ItemLightBehaviour : MonoBehaviour
    {
        [SerializeField] private float timeToGoOff = 1;
        private Light2D _light;
        private LightTwinkleBehaviour[] _twinkleBehaviours;

        private bool _isHiding;
        private float _currentIntensity;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _twinkleBehaviours = GetComponents<LightTwinkleBehaviour>();
        }

        private void Update()
        {
            if (_isHiding)
            {
                _light.intensity = _currentIntensity;
            }
        }

        public void PutOff()
        {
            _isHiding = true;
            
            foreach (var twinkleBehaviour in _twinkleBehaviours)
            {
                twinkleBehaviour.enabled = false;
            }

            _currentIntensity = _light.intensity;
            DOTween.To(() => _currentIntensity, x => _currentIntensity = x,
                0, timeToGoOff);
        }
    }
}