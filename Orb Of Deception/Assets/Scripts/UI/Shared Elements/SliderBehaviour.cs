using OrbOfDeception.Core;
using UnityEngine.UI;

namespace OrbOfDeception.UI
{
    public class SliderBehaviour : HideableElementAnimated
    {
        private Slider _button;

        protected override void Awake()
        {
            base.Awake();

            _button = GetComponent<Slider>();
        }

        private void Start()
        {
            _button.interactable = false;
        }

        public override void Show()
        {
            base.Show();

            _button.interactable = true;
        }

        public override void Hide()
        {
            base.Hide();

            _button.interactable = false;
        }
    }
}
