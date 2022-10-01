using System;
using System.Collections.Generic;
using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using OrbOfDeception.Items;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerMaskController : MonoBehaviour
    {
        public enum MaskType
        {
            ShinyMask, // Makes nothing.
            VigorousMask, // Orb goes further.
            ScarletMask // Orb moves faster.
        }
        
        public MaskItem[] masksInfo;
        
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private ParticleSystem _particles;

        private Dictionary<MaskType, MaskItem> masksDictionary;
        
        private static readonly int Change = Animator.StringToHash("Change");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _particles = GetComponentInChildren<ParticleSystem>();
            
            masksDictionary = new Dictionary<MaskType, MaskItem>();
            foreach (var maskInfo in masksInfo)
            {
                masksDictionary.Add(maskInfo.maskType, maskInfo);
            }
        }

        public void UpdateSprite()
        {
            var maskInfo = masksDictionary[SaveSystem.currentMaskType]; 
            
            _spriteRenderer.sprite = maskInfo.maskSprite;
            
            var main = _particles.main;
            main.startColor = maskInfo.particlesColor;
        }

        public void PlayMaskChangeAnimation()
        {
            _animator.SetTrigger(Change);
        }
    }
}
