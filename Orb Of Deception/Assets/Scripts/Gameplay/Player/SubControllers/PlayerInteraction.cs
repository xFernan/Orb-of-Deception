using System;

namespace OrbOfDeception.Player
{
    public class PlayerInteraction
    {
        public Action onInteraction;

        public void Start()
        {
            GameManager.InputManager.Interaction = OnInteraction;
        }

        private void OnInteraction()
        {
            if (GameManager.Player.isControlled) // Provisional
            {
                onInteraction?.Invoke();
            }
        }
    }
}