using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerTriggerCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameManager.Player.TriggerDetector.OnEnter(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            GameManager.Player.TriggerDetector.OnExit(other);
        }
    }
}
