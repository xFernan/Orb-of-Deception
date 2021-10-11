using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.CameraBehaviours
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlayerReferenceTransform;
        
        [HideInInspector] public Camera cameraComponent;
        [HideInInspector] public CameraLimits cameraLimits;
        [SerializeField] private float smoothSpeed = 10;
        private void Start()
        {
            cameraComponent = Camera.main;
        }

        private void Update()
        {
            if (cameraLimits == null) return;
            
            var cameraPlayerReferencePosition = cameraPlayerReferenceTransform.position;
            var cameraTransform = transform;
            
            var desiredPosition = new Vector3
            {
                x = Mathf.Clamp(cameraPlayerReferencePosition.x, cameraLimits.GetMinX(),
                    cameraLimits.GetMaxX()),
                y = Mathf.Clamp(cameraPlayerReferencePosition.y, cameraLimits.GetMinY(),
                    cameraLimits.GetMaxY()),
                z = cameraTransform.position.z
            };

            cameraTransform.position =
                Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }

        /*private void LateUpdate()
        {
            var cameraPosition = transform.position;
            
            var x = cameraPosition.x;
            x = Mathf.Round(x * 16) / 16;
            var y = cameraPosition.y;
            y = Mathf.Round(y * 16) / 16;
            var z = cameraPosition.z;

            transform.position = new Vector3(x, y, z);
        }*/

        public void Shake(float duration, float strength = 0.4f)
        {
            cameraComponent.DOShakePosition(duration, strength);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (cameraLimits != null)
                cameraLimits.DrawLimits();
        }
    }
}
