using DG.Tweening;
using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace OrbOfDeception
{
    public class SecretBreakableWallBehaviour : MonoBehaviour, IOrbCollisionable
    {
        private SpriteMaterialController _wallMaterial;
        private SpriteMaterialController _secretFrontMaterial;
        private TilemapCollider2D _wallCollider;

        private float _dissolveValue;
        private bool _isOpen;

        private const float FadeDuration = 0.75f;
        
        private void Start()
        {
            _wallCollider = GetComponent<TilemapCollider2D>();
            _wallMaterial = GetComponent<SpriteMaterialController>();
            _secretFrontMaterial = transform.GetChild(0).GetComponentInChildren<SpriteMaterialController>();
        }

        private void Update()
        {
            _wallMaterial.SetDissolve(_dissolveValue);
            _secretFrontMaterial.SetDissolve(_dissolveValue);
        }

        public void OnOrbCollisionEnter()
        {
            if (_isOpen) return;
            Open();
        }

        private void Open()
        {
            DOTween.To(()=> _dissolveValue, x=> _dissolveValue = x, 1, FadeDuration);
            _wallCollider.enabled = false;

            _isOpen = true;
        }
    }
}
