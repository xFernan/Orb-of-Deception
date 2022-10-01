using OrbOfDeception.CameraBehaviours;
using UnityEngine;

namespace OrbOfDeception.UI
{
    public class CanvasRenderOnVirtualCamera : MonoBehaviour
    {
        [SerializeField] private string sortingLayer;
        [SerializeField] private int sortingOrder;
        
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        
        private void Update()
        {
            _canvas.worldCamera = VirtualCamera.Instance.CameraComponent;
            _canvas.sortingLayerName = sortingLayer;
            _canvas.sortingOrder = sortingOrder;
            _canvas.planeDistance = 0;
        }
    }
}
