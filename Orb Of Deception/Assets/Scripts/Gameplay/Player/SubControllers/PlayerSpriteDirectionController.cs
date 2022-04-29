using System;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Player
{
    // PROVISIONAL
    public class PlayerSpriteDirectionController : MonoBehaviour
    {
        [HideInInspector] public int flip;
        private void Update()
        {
            if (PauseController.Instance.IsPaused)
                return;
            
            var flipFloat = GameManager.Instance.playerController.HorizontalMovementController.Direction;
            
            if (flipFloat == 0)
                return;
            
            flip = Math.Sign(flipFloat);
            
            transform.localScale = new Vector3(flip, 1, 1);
        }
    }
}
