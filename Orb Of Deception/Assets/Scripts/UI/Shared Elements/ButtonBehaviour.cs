using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI
{
    public class ButtonBehaviour : HideableElementAnimated
    {
        //[SerializeField] private UnityEvent onClick = new UnityEvent();

        [SerializeField] private UIArrowSelector[] arrows;

        private Button _button;

        protected override void Awake()
        {
            base.Awake();

            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.interactable = false;
        }

        public void OnPointerEnter()
        {
            if (!_button.interactable) return;
            
            ShowArrows();
        }

        public void OnPointerExit()
        {
            if (!_button.interactable) return;

            HideArrows();
        }

        private void ShowArrows()
        {
            foreach (var arrow in arrows)
            {
                arrow.Show();
            }
        }

        private void HideArrows()
        {
            foreach (var arrow in arrows)
            {
                arrow.Hide();
            }
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
            HideArrows();
        }
    }
}
