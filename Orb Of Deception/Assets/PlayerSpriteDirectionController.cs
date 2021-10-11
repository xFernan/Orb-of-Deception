using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    // PROVISIONAL
    public class PlayerSpriteDirectionController : MonoBehaviour
    {
        private void Update()
        {
            var flip = GameManager.Instance.playerController.HorizontalMovementController.Direction;
            
            if (flip == 0)
                return;
            
            flip = Mathf.Sign(flip);
            
            transform.localScale = new Vector3(flip, 1, 1);
        }
    }
}
