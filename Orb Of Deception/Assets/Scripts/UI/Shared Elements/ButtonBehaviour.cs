using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI
{
    public class ButtonBehaviour : HideableElementAnimated
    {
        //[SerializeField] private UnityEvent onClick = new UnityEvent();

        [SerializeField] private UIArrowSelector[] arrows;
        [SerializeField] private TextMeshProUGUI text;

        private Button _button;
        private SoundsPlayer _soundsPlayer;

        protected override void Awake()
        {
            base.Awake();

            _button = GetComponent<Button>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        private void Start()
        {
            _button.interactable = false;
            text.raycastTarget = false;
        }

        public void OnPointerEnter()
        {
            if (!_button.interactable) return;
            
            _soundsPlayer.Play("Hover");
            ShowArrows();
        }

        public void OnPointerExit()
        {
            if (!_button.interactable) return;

            HideArrows();
        }

        public void OnPointerClick()
        {
            _soundsPlayer.Play("Select");
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
            text.raycastTarget = true;
        }

        public override void Hide()
        {
            base.Hide();

            _button.interactable = false;
            text.raycastTarget = false;
            HideArrows();
        }
    }
}
