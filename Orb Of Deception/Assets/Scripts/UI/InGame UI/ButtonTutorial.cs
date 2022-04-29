using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI
{
    public class ButtonTutorial : MonoBehaviour
    {
        protected bool canBeShown = true;
        protected HideableElementAnimated buttonShower;

        private void Awake()
        {
            buttonShower = GetComponent<HideableElementAnimated>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!canBeShown) return;
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            buttonShower.Show();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            buttonShower.Hide();
        }

        protected virtual void Update()
        {
            
        }
    }
}
