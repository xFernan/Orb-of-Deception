using System;
using DG.Tweening;
using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using OrbOfDeception.Essence_of_Punishment;
using OrbOfDeception.Orb;
using OrbOfDeception.Rooms;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OrbOfDeception.Scenery_Elements
{
    public class BreakableDecorationBehaviour : MonoBehaviour, IOrbHittable
    {
        #region Variables

        [SerializeField] private bool isIrreparable = false;
        [SerializeField] private int decorationID;
        [SerializeField] private float dropProbability = 0.15f;
        [SerializeField] private int minDropAmount = 1;
        [SerializeField] private int maxDropAmount = 3;
        
        private SpriteMaterialController _materialController;
        private ParticleSystem _brokenParticles;
        private EssenceOfPunishmentSpawner _essenceOfPunishmentSpawner;
        private SoundsPlayer _soundsPlayer;
            
        private bool _isBroken;
        private float _dissolveValue;
        
        private const float DissolveDuration = 0.2f;
        
        #endregion
        
        #region Methods
        
        #region MonoBehaviour Methods
        
        private void Awake()
        {
            _brokenParticles = GetComponentInChildren<ParticleSystem>();
            _materialController = GetComponentInChildren<SpriteMaterialController>();
            _essenceOfPunishmentSpawner = GetComponentInChildren<EssenceOfPunishmentSpawner>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        private void Start()
        {
            if (isIrreparable)
            {
                if (!SaveSystem.IsIrreparableDecorationBroken(decorationID)) return;
            }
            else
            {
                if (!SaveSystem.IsDecorationBroken(decorationID)) return;
            }
            _dissolveValue = 1;
            _isBroken = true;
        }

        private void Update()
        {
            _materialController.SetDissolve(_dissolveValue);
        }

        #endregion
        
        #region Breakable Decoration Methods
        
        private void BreakDecoration()
        {
            _brokenParticles.Play();
            DOTween.To(()=> _dissolveValue, x=> _dissolveValue = x, 1, DissolveDuration);

            if (Random.Range(0.0f, 1.0f) <= dropProbability) DropEssenceOfPunishment();
            
            _soundsPlayer.Play("Destroying");
            
            if (isIrreparable)
                SaveSystem.AddIrreparableDecorationBroken(decorationID);
            else
                SaveSystem.AddDecorationBroken(decorationID);
            
            _isBroken = true;
        }

        private void DropEssenceOfPunishment()
        {
            var essenceOfPunishmentAmount =
                Random.Range(minDropAmount, maxDropAmount);
            _essenceOfPunishmentSpawner.SpawnEssences(essenceOfPunishmentAmount);
        }
        
        #endregion
        
        #region Implemented Methods
        
        public void OnOrbHitEnter(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0)
        {
            if (_isBroken)
            {
                _soundsPlayer.Play("Destroyed");
                return;
            }
            
            BreakDecoration();
        }
        
        #endregion
        
        #endregion
    }
}
