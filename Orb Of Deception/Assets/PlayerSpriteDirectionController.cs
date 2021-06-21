using System;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class PlayerSpriteDirectionController : MonoBehaviour
    {

        private PlayerController _playerController;
        
        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            var flip = _playerController.Direction;
            
            if (flip == 0)
                return;
            
            flip = Mathf.Sign(flip);
            
            transform.localScale = new Vector3(flip, 1, 1);
        }
    }
}
