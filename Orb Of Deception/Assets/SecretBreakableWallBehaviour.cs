using DG.Tweening;
using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace OrbOfDeception
{
    public class SecretBreakableWallBehaviour : MonoBehaviour, IOrbHittable
    {
        private SpriteMaterialController _wallMaterial;
        private SpriteMaterialController _secretFrontMaterial;
        private TilemapCollider2D _wallCollider;

        private float _opacityValue = 1;
        private bool _isOpen;

        private const float FadeDuration = 1f;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        
        private void Start()
        {
            _wallCollider = GetComponent<TilemapCollider2D>();
            _wallMaterial = GetComponent<SpriteMaterialController>();
            _secretFrontMaterial = transform.GetChild(0).GetComponentInChildren<SpriteMaterialController>();
        }

        private void Update()
        {
            _wallMaterial.SetOpacity(_opacityValue);
            _secretFrontMaterial.SetOpacity(_opacityValue);
        }

        public void Hit(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0)
        {
            if (_isOpen) return;
            Open();
        }

        private void Open()
        {
            DOTween.To(()=> _opacityValue, x=> _opacityValue = x, 0, FadeDuration);
            _wallCollider.enabled = false;

            _isOpen = true;
        }
    }
}
