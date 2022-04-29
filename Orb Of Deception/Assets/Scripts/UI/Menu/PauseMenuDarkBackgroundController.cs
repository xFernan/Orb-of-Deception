using DG.Tweening;
using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI.Menu
{
    public class PauseMenuDarkBackgroundController : HideableElement
    {
        [SerializeField] private float opacity;
        [SerializeField] private float opacityChangeDuration;
        
        private Image _image;
        private Tween _tween;

        private float _currentOpacity;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            var color = _image.color;
            color.a = _currentOpacity;
            _image.color = color;
        }

        public override void Show()
        {
            base.Show();
            
            var duration = opacity / (opacity - _currentOpacity) * opacityChangeDuration;
            
            _tween.Kill();
            _tween = DOTween.To(() => _currentOpacity, x => _currentOpacity = x,
                opacity, duration).SetUpdate(true);
        }

        public override void Hide()
        {
            base.Hide();
            
            var duration = opacity / (1 - (opacity - _currentOpacity)) * opacityChangeDuration;
            
            _tween.Kill();
            _tween = DOTween.To(() => _currentOpacity, x => _currentOpacity = x,
                0, duration).SetUpdate(true);
        }   
    }
}
