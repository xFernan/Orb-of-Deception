using System;
using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class CameraPlayerFollowerBehaviour : MonoBehaviour
    {
        [SerializeField] private float cameraMoveTimeWhenChangingDirection = 1;
        
        private float _offsetX;
        private Tween _currentTween;

        private void Awake()
        {
            HorizontalMovementController.onDirectionChanged += ChangeDirection;
        }

        private void Start()
        {
            _offsetX = transform.localPosition.x;
        }

        private void OnDestroy()
        {
            HorizontalMovementController.onDirectionChanged -= ChangeDirection;
        }

        private void ChangeDirection(int newDirection)
        {
            _currentTween.Kill();
            _currentTween = transform.DOLocalMoveX(newDirection * _offsetX, cameraMoveTimeWhenChangingDirection);
        }
        
    }
}
