using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public class LightController : MonoBehaviour
    {
        [SerializeField] private bool startsOff = false;
        [SerializeField] private float timeToGoOff = 1;
        private UnityEngine.Rendering.Universal.Light2D _light;
        private LightTwinkleBehaviour[] _twinkleBehaviours;

        private bool _isChanging = false;
        private float _currentIntensity;
        private float _defaultIntensity;

        private Tween _tween;
        private TweenCallback _tweenCallback;
        
        private void Awake()
        {
            _light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            _twinkleBehaviours = GetComponents<LightTwinkleBehaviour>();

            _tweenCallback += EnableLightTwinkleBehaviours;
        }

        private void Start()
        {
            _defaultIntensity = _light.intensity;
            
            if (startsOff)
            {
                foreach (var twinkleBehaviour in _twinkleBehaviours)
                {
                    twinkleBehaviour.enabled = false;
                }

                _light.intensity = 0;
            }
        }

        private void Update()
        {
            if (_isChanging)
            {
                _light.intensity = _currentIntensity;
            }
        }

        public void Hide()
        {
            Hide(timeToGoOff);
        }
        
        public void Hide(float timeHiding)
        {
            _isChanging = true;
            
            foreach (var twinkleBehaviour in _twinkleBehaviours)
            {
                twinkleBehaviour.enabled = false;
            }

            _currentIntensity = _light.intensity;
            
            _tween?.Kill();
            _tween = DOTween.To(() => _currentIntensity, x => _currentIntensity = x,
                0, timeHiding);
        }

        public void Appear()
        {
            Appear(timeToGoOff);
        }
        
        public void Appear(float timeAppearing)
        {
            _isChanging = true;
            
            foreach (var twinkleBehaviour in _twinkleBehaviours)
            {
                twinkleBehaviour.enabled = false;
            }

            _currentIntensity = _light.intensity;
            
            _tween?.Kill();
            _tween = DOTween.To(() => _currentIntensity, x => _currentIntensity = x,
                _defaultIntensity, timeAppearing);
            _tween.onComplete = EnableLightTwinkleBehaviours;
        }

        private void EnableLightTwinkleBehaviours()
        {
            foreach (var twinkleBehaviour in _twinkleBehaviours)
            {
                twinkleBehaviour.enabled = true;
            }
        }
    }
}
