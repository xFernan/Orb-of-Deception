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
        private ParticleSystem _hitParticles;
        private EssenceOfPunishmentSpawner _essenceOfPunishmentSpawner;
            
        private bool _hasAlreadyBeenHit;
        private float _dissolveValue;
        
        private const float DissolveDuration = 0.2f;
        private const float EssenceOfPunishmentDropProbability = 0.05f;
        private const int MinEssenceOfPunishmentDropAmount = 1;
        private const int MaxEssenceOfPunishmentDropAmount = 2;
        
        #endregion
        
        #region Methods
        
        #region MonoBehaviour Methods
        
        private void Awake()
        {
            _hitParticles = GetComponentInChildren<ParticleSystem>();
            _materialController = GetComponentInChildren<SpriteMaterialController>();
            _essenceOfPunishmentSpawner = GetComponentInChildren<EssenceOfPunishmentSpawner>();
        }

        public void OnOrbHitEnter(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0)
        {
            if (_hasAlreadyBeenHit) return;
            
            BreakDecoration();
            
            _hasAlreadyBeenHit = true;
        }
        
        private void Update()
        {
            _materialController.SetDissolve(_dissolveValue);
        }

        #endregion
        
        #region Breakable Decoration Methods
        
        private void BreakDecoration()
        {
            _hitParticles.Play();
            PlayerGroup.Camera.Shake(0.2f, 0.1f);
            DOTween.To(()=> _dissolveValue, x=> _dissolveValue = x, 1, DissolveDuration);

            if (Random.Range(0.0f, 1.0f) <= EssenceOfPunishmentDropProbability) DropEssenceOfPunishment();
        }

        private void DropEssenceOfPunishment()
        {
            var essenceOfPunishmentAmount =
                Random.Range(MinEssenceOfPunishmentDropAmount, MaxEssenceOfPunishmentDropAmount);
            _essenceOfPunishmentSpawner.SpawnEssences(essenceOfPunishmentAmount);
        }
        
        #endregion
        
        #endregion
    }
}