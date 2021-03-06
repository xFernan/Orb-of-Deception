using DG.Tweening;
using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace OrbOfDeception.Gameplay.Orb
{
    public class OrbLightColorChanger : MonoBehaviour
    {
        [SerializeField] private Color whiteOrbLightColor = Color.white;
        [SerializeField] private Color blackOrbLightColor = new Color(-0.65f, -0.65f, -0.65f);
        
        private Tween _currentTween;
        private Light2D _light;
        private Color _currentColor;

        private void Awake()
        {
            _currentColor = whiteOrbLightColor;
            _light = GetComponent<Light2D>();
        }

        private void Update()
        {
            _light.color = _currentColor;
        }

        public void ChangeLightColor(GameEntity.EntityColor orbColor)
        {
            var targetColor = orbColor == GameEntity.EntityColor.White ? whiteOrbLightColor : blackOrbLightColor;
            
            _currentTween.Kill();
            _currentTween = DOTween.To(() => _currentColor, x => _currentColor = x,
                targetColor, 0.25f);
        }
        
    }
}
