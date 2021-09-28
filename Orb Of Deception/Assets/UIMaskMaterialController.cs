using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception
{
    public class UIMaskMaterialController : MonoBehaviour
    {
        private Material _material;
        [HideInInspector] public float tintOpacity;

        private static readonly int TintOpacity = Shader.PropertyToID("_TintOpacity");
        
        private void Awake()
        {
            _material = Instantiate(GetComponent<Image>().material);
            GetComponent<Image>().material = _material;
        }
        
        private void Update()
        {
            _material.SetFloat(TintOpacity, tintOpacity);
        }
    }
}
