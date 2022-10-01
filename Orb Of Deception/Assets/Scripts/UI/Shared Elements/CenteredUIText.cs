using TMPro;
using UnityEngine;

namespace OrbOfDeception.UI
{
    public class CenteredUIText : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
        }

        void Update()
        {
            var newPosition = _originalPosition;
            newPosition.x += _text.renderedWidth % 2 != 0 ? 0.5f : 0;
            _rectTransform.anchoredPosition = newPosition;
        }
    }
}
