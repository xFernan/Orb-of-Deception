using System;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
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