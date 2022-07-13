using System;
using System.Collections.Generic;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerMaterialController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] sprites;
        
        private List<Material> _materials;

        [Range(0.0f, 1.0f)] public float tintOpacity;
        [Range(0.0f, 1.0f)] public float opacity;
        [Range(0.0f, 1.0f)] public float punishEffect;
        [Range(0.0f, 1.0f)] public float dissolve;

        private static readonly int TintColor = Shader.PropertyToID("_TintColor");
        private static readonly int TintOpacity = Shader.PropertyToID("_TintOpacity");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int PunishEffect = Shader.PropertyToID("_PunishEffect");
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
        private static readonly int UseBorderDuringDissolve = Shader.PropertyToID("_UseBorderDuringDissolve");

        private void Awake()
        {
            _materials = new List<Material>();
            foreach (var sprite in sprites)
            {
                _materials.Add(sprite.material);
            }
        }

        private void Start()
        {
            // Se inicializan los valores por defecto.
            tintOpacity = 0;
            opacity = 1;
            punishEffect = 0;
            dissolve = 0;
        }

        private void Update()
        {
            foreach (var material in _materials)
            {
                material.SetFloat(TintOpacity, tintOpacity);
                material.SetFloat(Opacity, opacity);
                material.SetFloat(PunishEffect, punishEffect);
                material.SetFloat(Dissolve, dissolve);
            }
        }

        private void LateUpdate()
        {
            var position = GameManager.Player.transform.position;
            
            var x = position.x;
            x = ((float)Mathf.RoundToInt(x * 16)) / 16;
            var y = position.y;
            y = ((float)Mathf.RoundToInt(y * 16)) / 16;
            var z = position.z;
            
            //transform.position = new Vector3(x, y, z);
        }

        private void SetTintColor(Color tintColor)
        {
            foreach (var material in _materials)
            {
                material.SetColor(TintColor, tintColor);
            }
        }

        public void SetTintToWhite()
        {
            SetTintColor(Color.white);
        }

        public void UseBorderOnDissolve()
        {
            foreach (var material in _materials)
            {
                material.SetInt(UseBorderDuringDissolve, 1);
            }
        }
    }
}
