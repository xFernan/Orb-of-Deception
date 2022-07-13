using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraRealLimits : MonoBehaviour
    {
        private Transform _limitSouthWest;
        private Transform _limitNorthEast;
        
        private const float TweenDuration = 0.5f;
        private Tween _southWestTween, _northEastTween;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            var transformComponent = transform;
            
            _limitSouthWest = new GameObject().transform;
            _limitSouthWest.parent = transformComponent;
            
            _limitNorthEast = new GameObject().transform;
            _limitNorthEast.parent = transformComponent;
        }

        public void SetNewLimits(CameraLimits cameraLimits)
        {
            var newLimitSouthWestPosition = cameraLimits.GetSouthWestLimit();
            var newLimitNorthEastPosition = cameraLimits.GetNorthEastLimit();
            
            _limitSouthWest.position = newLimitSouthWestPosition;
            _limitNorthEast.position = newLimitNorthEastPosition;
        }
        
        public void LerpToNewLimits(CameraLimits cameraLimits)
        {
            var newLimitSouthWestPosition = cameraLimits.GetSouthWestLimit();
            var newLimitNorthEastPosition = cameraLimits.GetNorthEastLimit();
            
            _southWestTween?.Kill();
            _limitSouthWest.DOMove(newLimitSouthWestPosition, TweenDuration).SetEase(Ease.OutQuint);
            _northEastTween?.Kill();
            _limitNorthEast.DOMove(newLimitNorthEastPosition, TweenDuration).SetEase(Ease.OutQuint);
        }

        public float GetMinX()
        {
            return _limitSouthWest.position.x;
        }
        
        public float GetMaxX()
        {
            return _limitNorthEast.position.x;
        }

        public float GetMinY()
        {
            return _limitSouthWest.position.y;
        }
        
        public float GetMaxY()
        {
            return _limitNorthEast.position.y;
        }
    }
}