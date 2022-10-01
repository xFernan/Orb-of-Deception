using System;
using DG.Tweening;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlayerReferenceTransform;
        
        [HideInInspector] public Camera cameraComponent;
        [HideInInspector] public CameraLimits cameraLimits;
        [SerializeField] private float smoothSpeedX = 10;
        [SerializeField] private float smoothSpeedY = 30;

        private CameraRealLimits _cameraRealLimits;
        
        private Tween _shakeTween;
        private Tween _returningTween;
        
        private Vector3 _realPosition;

        private void Awake()
        {
            _cameraRealLimits = new GameObject().AddComponent<CameraRealLimits>();
        }

        private void Start()
        {
            cameraComponent = GetComponentInChildren<Camera>();
            _realPosition = transform.position;
        }

        private void Update()
        {
            if (cameraLimits == null) return;
            
            var cameraPlayerReferencePosition = cameraPlayerReferenceTransform.position;
            //var cameraTransform = transform;
            
            var targetPosition = new Vector3
            {
                x = Mathf.Clamp(cameraPlayerReferencePosition.x, _cameraRealLimits.GetMinX(),
                    _cameraRealLimits.GetMaxX()),
                y = Mathf.Clamp(cameraPlayerReferencePosition.y, _cameraRealLimits.GetMinY(),
                    _cameraRealLimits.GetMaxY()),
                z = -10
            };

            _realPosition = new Vector3(Mathf.Lerp(_realPosition.x, targetPosition.x, smoothSpeedX * Time.deltaTime),
                Mathf.Lerp(_realPosition.y, targetPosition.y, smoothSpeedY * Time.deltaTime), _realPosition.z);

            var snappedPosition = new Vector3(SnapToGrid.SnapValueToGrid(targetPosition.x),
                SnapToGrid.SnapValueToGrid(targetPosition.y),
                -10);

            transform.position = targetPosition;

            /*var cameraPosition = cameraTransform.position;
            
            var x = cameraPosition.x;
            x = ((float)Mathf.RoundToInt(x * 16)) / 16;
            var y = cameraPosition.y;
            y = ((float)Mathf.RoundToInt(y * 16)) / 16;
            var z = cameraPosition.z;

            Debug.Log(cameraPosition.x + "--> " + x);
            cameraTransform.position = new Vector3(x, y, z);*/
        }
        
        public void SetNewCameraLimits(CameraLimits newCameraLimits)
        {
            if (newCameraLimits == null) return;
            
            cameraLimits = newCameraLimits;
            _cameraRealLimits.SetNewLimits(newCameraLimits);
        }
        
        public void LerpToNewCameraLimits(CameraLimits newCameraLimits)
        {
            if (newCameraLimits == null) return;
            
            cameraLimits = newCameraLimits;
            _cameraRealLimits.LerpToNewLimits(newCameraLimits);
        }
        
        public void RePosition()
        {
            var cameraPlayerReferencePosition = cameraPlayerReferenceTransform.position;
            /*var cameraTransform = transform;
            
            cameraPlayerReferencePosition.z = cameraTransform.position.z;
            
            cameraTransform.position = cameraPlayerReferencePosition;*/
            
            transform.position = new Vector3
            {
                x = Mathf.Clamp(cameraPlayerReferencePosition.x, cameraLimits.GetMinX(),
                    cameraLimits.GetMaxX()),
                y = Mathf.Clamp(cameraPlayerReferencePosition.y, cameraLimits.GetMinY(),
                    cameraLimits.GetMaxY()),
                z = -10
            };
        }
        
        public void Shake(float duration, float strength = 0.4f)
        {
            _returningTween?.Kill();
            
            cameraComponent.transform.localPosition = Vector3.zero;
            
            _shakeTween?.Kill();
            _shakeTween = cameraComponent.DOShakePosition(duration, strength).OnComplete(OnShakeEnded);
        }

        private void OnShakeEnded()
        {
            _returningTween?.Kill();
            _returningTween = cameraComponent.transform.DOLocalMove(Vector3.zero, 0.75f);
        }
    }
}
