using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI
{
    public class SliderBehaviour : HideableElementAnimated
    {
        [SerializeField] private Image handle;
        
        private Slider _button;

        protected override void Awake()
        {
            base.Awake();

            _button = GetComponent<Slider>();
        }

        private void Start()
        {
            _button.interactable = false;
            handle.raycastTarget = false;
        }

        public override void Show()
        {
            base.Show();

            _button.interactable = true;
            handle.raycastTarget = true;
        }

        public override void Hide()
        {
            base.Hide();

            _button.interactable = false;
            handle.raycastTarget = false;
        }
    }
}
