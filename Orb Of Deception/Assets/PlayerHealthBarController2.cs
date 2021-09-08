using OrbOfDeception.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception
{
    public class PlayerHealthBarController2 : MonoBehaviour
    {
        /*[SerializeField] private Image leftLimit;
        [SerializeField] private Image rightLimit;

        [Space]

        [SerializeField] private Sprite filledMask;
        [SerializeField] private Sprite emptyMask;
        
        private void Awake()
        {
            PlayerHealthController.onDamage += OnDamage;
        }
        
        private void Start()
        {
            _healthBarValue = PlayerGroup.Player.PlayerHealthController.GetCurrentHealth();
        }

        private void OnDestroy()
        {
            PlayerHealthController.onDamage -= OnDamage;
        }
        
        private void OnDamage()
        {
            _currentTween.Kill();
            _currentTween = DOTween.To(()=> _healthBarValue, x=> _healthBarValue = x, newHealth, TweenDuration);
            
            ChangeRectTransformWidth(healthBarFillLeftover, _healthBarValue * HealthValueMultiplier);
            _healthBarFillLeftoverAnimator.SetTrigger(Fill);
            Invoke(nameof(ShowHealthBarFillLeftover), TweenDuration);
        }*/
        
    }
}
