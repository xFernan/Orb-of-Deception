using System;
using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using UnityEngine;

namespace OrbOfDeception
{
    public class CollectibleController : MonoBehaviour, IOrbHittable
    {
        [SerializeField] private float floatingMaxDistance;
        [SerializeField] private float floatingVelocity;
        [SerializeField] protected ParticleSystem idleParticles;
        [SerializeField] private MultipleParticlesController hideParticles;
        
        public float tintOpacity;
        private bool _hasBeenCollected;
        private Vector3 _startingPosition;

        private ItemLightBehaviour _itemLightBehaviour;
        private SpriteMaterialController _spriteMaterialController;
        private Animator _animator;
        private Collider2D _collider;
        
        private static readonly int HideTrigger = Animator.StringToHash("Hide");

        private void Awake()
        {
            _itemLightBehaviour = GetComponentInChildren<ItemLightBehaviour>();
            _spriteMaterialController = GetComponentInChildren<SpriteMaterialController>();
            _spriteMaterialController.SetTintColor(Color.white);
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _startingPosition = transform.position;
        }

        private void Update()
        {
            _spriteMaterialController.SetTintOpacity(tintOpacity);
            transform.position = _startingPosition + Vector3.up * floatingMaxDistance * Mathf.Sin(Time.time * floatingVelocity);
        }

        public void OnOrbHitEnter(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0)
        {
            if (!_hasBeenCollected)
                Collect();
        }

        private void Collect()
        {
            _animator.SetTrigger(HideTrigger);
            idleParticles.Stop();
            hideParticles.Play();
            _itemLightBehaviour.PutOff();
            _collider.enabled = false;
            _hasBeenCollected = true;
            
            CollectEffect();
        }

        protected virtual void CollectEffect()
        {
            
        }
    }
}