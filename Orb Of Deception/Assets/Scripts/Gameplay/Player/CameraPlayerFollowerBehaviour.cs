using System;
using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class CameraPlayerFollowerBehaviour : MonoBehaviour
    {
        [SerializeField] private float offsetIdle = 0.25f;
        [SerializeField] private float offsetMoving = 0.75f;

        private const float LerpVelocity = 0.25f;

        private void FixedUpdate()
        {
            var playerMovementController = GameManager.Player.HorizontalMovementController;
            var targetPositionX = playerMovementController.Orientation * (playerMovementController.IsMoving ? offsetMoving : offsetIdle);

            var localPosition = transform.localPosition;
            localPosition.x = Mathf.Lerp(localPosition.x, targetPositionX, LerpVelocity);
            transform.localPosition = localPosition;
        }
    }
}
