using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class PlayerMaterialController : MonoBehaviour
    {
        private Material _material;

        [Range(0.0f, 1.0f)] public float tintOpacity;
        [Range(0.0f, 1.0f)] public float opacity;

        private static readonly int TintOpacity = Shader.PropertyToID("_TintOpacity");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int TintIsWhite = Shader.PropertyToID("_TintIsWhite");

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Start()
        {
            // Se inicializan los valores por defecto.
            tintOpacity = 0;
            opacity = 1;
            
            _material.SetInt(TintIsWhite, 0);
        }

        private void Update()
        {
            _material.SetFloat(TintOpacity, tintOpacity);
            _material.SetFloat(Opacity, opacity);
        }
    }
}
