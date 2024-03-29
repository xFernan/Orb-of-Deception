﻿using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.Enemy
{
    public class ImageMaterialController : MonoBehaviour
    {
        private Material _material;
        
        private static readonly int TintColor = Shader.PropertyToID("_TintColor");
        private static readonly int TintOpacity = Shader.PropertyToID("_TintOpacity");
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int PunishEffect = Shader.PropertyToID("_PunishEffect");
        private static readonly int UseBorderDuringDissolve = Shader.PropertyToID("_UseBorderDuringDissolve");

        private void Awake()
        {
            _material = GetComponent<Image>().material;
        }

        public void SetTintColor(Color tintColor)
        {
            _material.SetColor(TintColor, tintColor);
        }

        public void SetTintOpacity(float tintOpacity)
        {
            _material.SetFloat(TintOpacity, tintOpacity);
        }
        
        public void SetDissolve(float dissolve)
        {
            _material.SetFloat(Dissolve, dissolve);
        }

        public void SetOpacity(float opacity)
        {
            _material.SetFloat(Opacity, opacity);
        }
        
        public void SetPunishEffect(float punishEffect)
        {
            _material.SetFloat(PunishEffect, punishEffect);
        }

        public void SetUseBorderDuringDissolve(bool useBorderDuringDissolve)
        {
            _material.SetInt(UseBorderDuringDissolve, useBorderDuringDissolve ? 1 : 0);
        }
    }
}