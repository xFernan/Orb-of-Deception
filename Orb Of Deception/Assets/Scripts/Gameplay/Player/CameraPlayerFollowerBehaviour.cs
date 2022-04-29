using UnityEngine;

namespace OrbOfDeception.Player
{
    public class CameraPlayerFollowerBehaviour : MonoBehaviour
    {
        private float _offsetX;

        private void Awake()
        {
            PlayerHorizontalMovementController.onDirectionChanged += ChangeDirection;
        }

        private void Start()
        {
            _offsetX = transform.localPosition.x;
        }

        private void OnDestroy()
        {
            PlayerHorizontalMovementController.onDirectionChanged -= ChangeDirection;
        }

        private void ChangeDirection(int newDirection)
        {
            var cameraTransform = transform;
            
            var position = cameraTransform.localPosition;
            position.x = newDirection * _offsetX;
            cameraTransform.localPosition = position;
        }
        
    }
}
