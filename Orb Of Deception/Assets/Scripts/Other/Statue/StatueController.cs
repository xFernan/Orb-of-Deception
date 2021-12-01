using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception.Statue
{
    public class StatueController : MonoBehaviour
    {
        [SerializeField] private HideableElement interactIndicator;
        private bool _isActive;

        private void Awake()
        {
            interactIndicator = GetComponentInChildren<HideableElement>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _isActive = true;
            interactIndicator.Show();
            GameManager.Player.PlayerInteraction.onInteraction += Pray;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            interactIndicator.Hide();
            GameManager.Player.PlayerInteraction.onInteraction -= Pray;
            _isActive = false;
        }

        private void Pray()
        {
            var player = GameManager.Player;
            player.KneelController.Kneel();
            player.PlayerHealthController.FullHealth();
        }
    }
}
