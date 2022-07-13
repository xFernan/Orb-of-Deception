using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraLimitsChanger : MonoBehaviour
    {
        [SerializeField] private CameraLimits leftSideCameraLimits;
        [SerializeField] private CameraLimits rightSideCameraLimits;
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<PlayerTriggerCollider>() == null) return;

            var playerPosition = GameManager.Player.transform.position;
            
            GameManager.Camera.LerpToNewCameraLimits(
                playerPosition.x < transform.position.x ? leftSideCameraLimits : rightSideCameraLimits);
        }
    }
}
