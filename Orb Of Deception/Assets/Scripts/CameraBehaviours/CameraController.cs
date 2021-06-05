using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlayerReferenceTransform;
        private Camera _camera;
        private CameraLimits _cameraLimits;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var cameraPlayerReferencePosition = cameraPlayerReferenceTransform.position;
            var cameraTransform = transform;

            var height = _camera.orthographicSize * 2.0f;
            var width = height * _camera.aspect;
            
            var newCameraPosition = new Vector3
            {
                x = Mathf.Clamp(cameraPlayerReferencePosition.x, _cameraLimits.GetMinX() + width / 2,
                    _cameraLimits.GetMaxX() - width / 2),
                y = Mathf.Clamp(cameraPlayerReferencePosition.y, _cameraLimits.GetMinY() + height / 2,
                    _cameraLimits.GetMaxY() - height / 2),
                z = cameraTransform.position.z
            };

            cameraTransform.position = newCameraPosition;
        }

        public void UpdateCameraLimits(CameraLimits newCameraLimits)
        {
            _cameraLimits = newCameraLimits;
        }
        
    }
}
