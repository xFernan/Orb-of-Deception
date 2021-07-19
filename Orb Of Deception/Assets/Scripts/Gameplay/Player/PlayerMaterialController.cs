using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class PlayerMaterialController : MonoBehaviour
    {
        private Material _material;

        [Range(0.0f, 1.0f)] public float tintOpacity;
        [Range(0.0f, 1.0f)] public float opacity;
        [Range(0.0f, 1.0f)] public float punishEffect;

        private static readonly int TintColor = Shader.PropertyToID("_TintColor");
        private static readonly int TintOpacity = Shader.PropertyToID("_TintOpacity");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int PunishEffect = Shader.PropertyToID("_PunishEffect");

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Start()
        {
            // Se inicializan los valores por defecto.
            tintOpacity = 0;
            opacity = 1;
            punishEffect = 0;
        }

        private void Update()
        {
            _material.SetFloat(TintOpacity, tintOpacity);
            _material.SetFloat(Opacity, opacity);
            _material.SetFloat(PunishEffect, punishEffect);
        }
        
        private void SetTintColor(Color tintColor)
        {
            _material.SetColor(TintColor, tintColor);
        }

        public void SetTintToWhite()
        {
            SetTintColor(Color.white);
        }
    }
}
