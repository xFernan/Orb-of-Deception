using UnityEngine;

namespace OrbOfDeception.UI
{
    public class SnapToGridUI : MonoBehaviour
    {
        //[SerializeField] private Transform targetTransformPosition;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            var position = _rectTransform.anchoredPosition;
            position.x = SnapValueToGrid(position.x);
            position.y = SnapValueToGrid(position.y);
            _rectTransform.anchoredPosition = position;
        }

        public static float SnapValueToGrid(float value)
        {
            const float ppu = (float)GameManager.Ppu;
            return Mathf.FloorToInt(value * ppu) / ppu;
        }
    }
}
