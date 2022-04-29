using System.Collections;
using OrbOfDeception.Core;
using TMPro;
using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI
{
    public class TitleDisplayer : MonoBehaviour
    {
        [SerializeField] private float delayToShow = 1;
        [SerializeField] private float timeShown = 3;

        private TextMeshProUGUI _textMeshPro;
        
        private HideableElementAnimated[] _elements;
        private Coroutine _areaTitleDisplayCoroutine;
        
        private void Awake()
        {
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            _elements = GetComponentsInChildren<HideableElementAnimated>();
            foreach (var element in _elements)
            {
                var animator = element.GetComponent<Animator>();
                animator.speed = 0.8f;
            }
        }

        public void DisplayTitle(string areaTitle)
        {
            _textMeshPro.text = areaTitle;
            
            if (_areaTitleDisplayCoroutine != null)
            {
                StopCoroutine(DisplayTitleCoroutine());
            }
            
            foreach (var element in _elements)
            {
                element.SetHidden();
            }
            
            _areaTitleDisplayCoroutine = StartCoroutine(DisplayTitleCoroutine());
        }

        private IEnumerator DisplayTitleCoroutine()
        {
            yield return new WaitForSeconds(delayToShow);
            
             Show();

             yield return new WaitForSeconds(timeShown);
             
             Hide();
        }
        
        private void Show()
        {
            foreach (var element in _elements)
            {
                element.Show();
            }
        }

        private void Hide()
        {
            foreach (var element in _elements)
            {
                element.Hide();
            }
        }
    }
}
