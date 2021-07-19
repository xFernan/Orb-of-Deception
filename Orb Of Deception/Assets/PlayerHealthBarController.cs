using DG.Tweening;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class PlayerHealthBarController : MonoBehaviour
    {
        [SerializeField] private int healthBarBorderWidthDifferent = 2;
        [SerializeField] private RectTransform healthBarBorder;
        [SerializeField] private RectTransform healthBarFill;
        [SerializeField] private RectTransform healthBarFillLeftover;

        private Animator _healthBarFillLeftoverAnimator;
        
        private float _healthBarValue;
        private Tween _currentTween;
        private static readonly int Fade = Animator.StringToHash("Fade");
        private static readonly int Fill = Animator.StringToHash("Fill");

        private const float HealthValueMultiplier = 1.2f;
        private const float TweenDuration = 0.3f;

        private void Awake()
        {
            _healthBarFillLeftoverAnimator = healthBarFillLeftover.GetComponent<Animator>();
            
            PlayerHealthController.onHealthChange += OnHealthChange;
        }
        
        private void Start()
        {
            _healthBarValue = PlayerGroup.Player.PlayerHealthController.GetCurrentHealth();
        }

        private void Update()
        {
            ChangeRectTransformWidth(healthBarBorder,
                PlayerGroup.Player.initialHealth * HealthValueMultiplier + healthBarBorderWidthDifferent);
            ChangeRectTransformWidth(healthBarFill,
                _healthBarValue * HealthValueMultiplier);
        }

        private void OnDestroy()
        {
            PlayerHealthController.onHealthChange -= OnHealthChange;
        }
        
        private void OnHealthChange(int newHealth)
        {
            _currentTween.Kill();
            _currentTween = DOTween.To(()=> _healthBarValue, x=> _healthBarValue = x, newHealth, TweenDuration);
            
            ChangeRectTransformWidth(healthBarFillLeftover, _healthBarValue * HealthValueMultiplier);
            _healthBarFillLeftoverAnimator.SetTrigger(Fill);
            Invoke(nameof(ShowHealthBarFillLeftover), TweenDuration);
        }

        private static void ChangeRectTransformWidth(RectTransform rectTransform, float newWidth)
        {
            var newSize = rectTransform.sizeDelta;
            newSize.x = newWidth;
            rectTransform.sizeDelta = newSize;
        }
        
        private void ShowHealthBarFillLeftover()
        {
            _healthBarFillLeftoverAnimator.SetTrigger(Fade);
        }
    }
}
