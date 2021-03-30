using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlayerReferenceTransform;
        [SerializeField] private CameraLimits cameraLimits;

        private void Update()
        {
            var cameraPlayerReferencePosition = cameraPlayerReferenceTransform.position;
            var cameraTransform = transform;

            /*var camera = Camera.main;
            var height = camera.orthographicSize * 2.0f;
            var width = height * camera.aspect;*/
            
            var newCameraPosition = new Vector3
            {
                x = Mathf.Clamp(cameraPlayerReferencePosition.x, cameraLimits.GetMinX(),
                    cameraLimits.GetMaxX()),
                y = Mathf.Clamp(cameraPlayerReferencePosition.y, cameraLimits.GetMinY(),
                    cameraLimits.GetMaxY()),
                z = cameraTransform.position.z
            };

            cameraTransform.position = newCameraPosition;
        }
    }
}
