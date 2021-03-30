using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class CameraFollowerBehaviour : MonoBehaviour
    {
        private float _offsetX;
        private int currentDirection;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float cameraMoveTimeWhenChangingDirection = 1;

        private void Start()
        {
            _offsetX = transform.localPosition.x;
            currentDirection = (_offsetX >= 0) ? 1 : -1;
        }

        private void Update()
        {
            var playerDirection = (int) playerController.Direction;
            if (playerDirection != 0 && playerDirection != currentDirection)
            {
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            currentDirection = -currentDirection;
            transform.DOLocalMoveX(currentDirection * _offsetX, cameraMoveTimeWhenChangingDirection);
        }
    }
}
