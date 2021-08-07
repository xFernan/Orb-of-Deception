using System;
using DG.Tweening;
using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class BreakableDecorationBehaviour : MonoBehaviour, IOrbHittable
    {
        #region Variables
        private SpriteMaterialController _materialController;
        
        private bool _hasAlreadyBeenHit;
        private float _dissolveValue;
        private const float DissolveDuration = 0.2f;
        
        private ParticleSystem _hitParticles;
        #endregion
        
        #region Methods
        
        #region MonoBehaviour Methods
        private void Awake()
        {
            _hitParticles = GetComponentInChildren<ParticleSystem>();
            _materialController = GetComponentInChildren<SpriteMaterialController>();
        }

        public void OnOrbHitEnter(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0)
        {
            if (_hasAlreadyBeenHit) return;
            
            _hitParticles.Play();
            PlayerGroup.Camera.Shake(0.2f, 0.1f);
            DOTween.To(()=> _dissolveValue, x=> _dissolveValue = x, 1, DissolveDuration);
            
            _hasAlreadyBeenHit = true;
        }

        private void Update()
        {
            _materialController.SetDissolve(_dissolveValue);
        }

        #endregion
        
        #endregion
    }
}
