using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlayerReferenceTransform;
        private CameraLimits cameraLimits;

        private void Update()
        {
            var cameraPlayerReferencePosition = cameraPlayerReferenceTransform.position;
            var cameraTransform = transform;

            var camera = Camera.main;
            var height = camera.orthographicSize * 2.0f;
            var width = height * camera.aspect;
            
            Debug.Log(cameraLimits.GetMinX() + " " + cameraLimits.GetMaxX() + " | " + cameraLimits.GetMinY() + " " + cameraLimits.GetMaxY());
            
            var newCameraPosition = new Vector3
            {
                x = Mathf.Clamp(cameraPlayerReferencePosition.x, cameraLimits.GetMinX() + width / 2,
                    cameraLimits.GetMaxX() - width / 2),
                y = Mathf.Clamp(cameraPlayerReferencePosition.y, cameraLimits.GetMinY() + height / 2,
                    cameraLimits.GetMaxY() - height / 2),
                z = cameraTransform.position.z
            };

            cameraTransform.position = newCameraPosition;
        }

        public void UpdateCameraLimits(CameraLimits newCameraLimits)
        {
            cameraLimits = newCameraLimits;
        }
        
    }
}
