using System.Collections;
using OrbOfDeception.Audio;
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
        private SoundsPlayer _soundsPlayer;
        
        private HideableElementAnimated[] _elements;
        private Coroutine _areaTitleDisplayCoroutine;
        
        private void Awake()
        {
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
            _elements = GetComponentsInChildren<HideableElementAnimated>();
            foreach (var element in _elements)
            {
                var animator = element.GetComponent<Animator>();
                animator.speed = 0.8f;
            }
        }

        public void DisplayTitle(string areaTitle, bool playDisplaySound = true)
        {
            _textMeshPro.text = areaTitle;
            
            if (_areaTitleDisplayCoroutine != null)
            {
                StopCoroutine(DisplayTitleCoroutine(playDisplaySound));
            }
            
            foreach (var element in _elements)
            {
                element.SetHidden();
            }
            
            _areaTitleDisplayCoroutine = StartCoroutine(DisplayTitleCoroutine(playDisplaySound));
        }

        private IEnumerator DisplayTitleCoroutine(bool playDisplaySound)
        {
             yield return new WaitForSeconds(delayToShow);
            
             Show(playDisplaySound);

             yield return new WaitForSeconds(timeShown);
             
             Hide();
        }
        
        private void Show(bool playDisplaySound)
        {
            foreach (var element in _elements)
            {
                element.Show();
            }
            
            if (playDisplaySound)
                _soundsPlayer.Play("Display");
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
