using System.Collections;
using OrbOfDeception.Gameplay.Player;
using TMPro;
using UnityEngine;

namespace OrbOfDeception.UI.Essence_of_Punishment_Counter
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

        private void Awake()
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

        protected void ShowCounter()
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);
            
            _showCoroutine = StartCoroutine(ShowCounterCoroutine());
        }

        private IEnumerator ShowCounterCoroutine()
        {
            _isVisible = true;
            yield return new WaitForSeconds(timeBeingShowed);
            _isVisible = false;
        }
    }
}
