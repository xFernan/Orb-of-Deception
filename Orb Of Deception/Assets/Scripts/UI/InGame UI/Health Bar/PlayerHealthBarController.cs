using System.Collections.Generic;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI.Health_Bar
{
    public class PlayerHealthBarController : MonoBehaviour
    {
        [SerializeField] private GameObject oddConnector;
        [SerializeField] private GameObject evenConnector;
        [SerializeField] private GameObject endConnector;
        [SerializeField] private GameObject maskPrefab;

        private RectTransform _rectTransform;

        private const int MaskWidth = 7;
        private const int NewMaskBlockDistance = 11;
        
        private List<UIHeartController> _masks;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _masks = new List<UIHeartController>();
            
            PlayerHealthController.onPlayerDamage += OnPlayerDamage;
        }

        private void Start()
        {
            BuildBar();
        }

        private void BuildBar()
        {
            const int maskAmount = PlayerController.InitialHealth; // Provisional.
            var originPosition = _rectTransform.anchoredPosition;
            
            for (var i = 0; i < maskAmount; i++)
            {
                var newMask = Instantiate(maskPrefab, transform, true);
                newMask.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(NewMaskBlockDistance * i, -1);
                _masks.Add(newMask.GetComponent<UIHeartController>());
                
                var connectorToInstantiate = (i == maskAmount - 1) ?
                    endConnector :
                    ((i) % 2 == 0) ? oddConnector : evenConnector;
                var connectorObject = Instantiate(connectorToInstantiate, transform, true);
                connectorObject.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(MaskWidth + NewMaskBlockDistance * i, 0);
            }
        }

        private void OnPlayerDamage()
        {
            var maskID = GameManager.Player.HealthController.GetCurrentHealth();
            _masks[maskID].Break();
        }

        public void RecoverAll()
        {
            foreach (var mask in _masks)
            {
                if (mask.IsBroken())
                    mask.Recover();
            }
        }
        
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
