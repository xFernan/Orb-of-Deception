using DG.Tweening;
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
                    cameraLimits.GetMaxY())
            };

            var camPosition = cameraTransform.position;
            cameraTransform.position = new Vector3(Mathf.Lerp(camPosition.x, desiredPosition.x, smoothSpeedX * Time.deltaTime),
                Mathf.Lerp(camPosition.y, desiredPosition.y, smoothSpeedY * Time.deltaTime), camPosition.z);
            
            /*var cameraPosition = cameraTransform.position;
            
            var x = cameraPosition.x;
            x = ((float)Mathf.RoundToInt(x * 16)) / 16;
            var y = cameraPosition.y;
            y = ((float)Mathf.RoundToInt(y * 16)) / 16;
            var z = cameraPosition.z;

            Debug.Log(cameraPosition.x + "--> " + x);
            cameraTransform.position = new Vector3(x, y, z);*/
        }

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
