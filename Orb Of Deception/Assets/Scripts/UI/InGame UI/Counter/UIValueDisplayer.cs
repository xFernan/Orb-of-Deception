using System.Collections;
using TMPro;
using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI.Counter
{
    public class UIValueDisplayer : MonoBehaviour
    {
        [SerializeField] private float lerpVelocity;
        [SerializeField] private float timeBeingShowed = 3;

        private Animator _animator;
        private TextMeshProUGUI _displayText;

        private Coroutine _showCoroutine;
        protected float _showedValue;
        private bool _isVisible;
        
        protected virtual float targetValueToShow { get; set; }
        
        private static readonly int IsVisible = Animator.StringToHash("IsVisible");

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _displayText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            _animator.SetBool(IsVisible, _isVisible);
            
            _showedValue = Mathf.Lerp(_showedValue, targetValueToShow, lerpVelocity * Time.deltaTime);
            _displayText.text = Mathf.RoundToInt(_showedValue) + "";
        }

        public void ShowCounterBriefly()
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);
            
            _showCoroutine = StartCoroutine(ShowCounterCoroutine());
        }

        public void ShowCounter()
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);

            _isVisible = true;
        }

        public void HideCounter()
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);

            _isVisible = false;
        }
        
        private IEnumerator ShowCounterCoroutine()
        {
            _isVisible = true;
            yield return new WaitForSeconds(timeBeingShowed);
            _isVisible = false;
        }
    }
}
