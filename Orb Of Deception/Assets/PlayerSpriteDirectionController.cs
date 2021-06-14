using System;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class PlayerSpriteDirectionController : MonoBehaviour
    {

        private PlayerController _playerController;
        
        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
            PlayerController.onDirectionChanged += ChangeSpriteDirection;
        }

        private void OnDestroy()
        {
            PlayerController.onDirectionChanged -= ChangeSpriteDirection;
        }

        private void ChangeSpriteDirection(int newDirection)
        {
            transform.localScale = new Vector3(newDirection, 1, 1);
        }
        
    }
}
