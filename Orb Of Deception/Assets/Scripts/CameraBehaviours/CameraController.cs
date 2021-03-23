using System;
using UnityEngine;

namespace Nanref.CameraBehaviours
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlayerReferenceTransform;
        [SerializeField] private CameraLimits cameraLimits;

        private void Update()
        {
            var cameraPlayerReferencePosition = cameraPlayerReferenceTransform.position;
            var cameraTransform = transform;
            
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
