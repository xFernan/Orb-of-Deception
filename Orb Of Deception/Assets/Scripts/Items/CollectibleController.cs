using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using OrbOfDeception.Orb;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Items
{
    public class CollectibleController : MonoBehaviour, IOrbHittable, IPlayerHittable
    {
        [SerializeField] private float floatingMaxDistance;
        [SerializeField] private float floatingVelocity;
        [SerializeField] protected ParticleSystem idleParticles;
        [SerializeField] protected MultipleParticlesController onGetParticles;
        
        public float tintOpacity;
        private bool _hasBeenCollected;
        private Vector3 _startingPosition;

        private ItemLightBehaviour _itemLightBehaviour;
        private SpriteMaterialController _spriteMaterialController;
        protected Animator animator;
        private Collider2D _collider;
        
        private static readonly int HideTrigger = Animator.StringToHash("Hide");

        protected virtual void Awake()
        {
            _itemLightBehaviour = GetComponentInChildren<ItemLightBehaviour>();
            _spriteMaterialController = GetComponentInChildren<SpriteMaterialController>();
            _spriteMaterialController.SetTintColor(Color.white);
            animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
        }

        protected virtual void Start()
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
            Collect();
        }

        public void OnPlayerHitEnter()
        {
            Collect();
        }

        public void OnPlayerHitExit()
        {
            
        }

        private void Collect()
        {
            if (_hasBeenCollected)
                return;
            animator.SetTrigger(HideTrigger);
            idleParticles.Stop();
            onGetParticles.Play();
            _itemLightBehaviour.PutOff();
            _collider.enabled = false;
            _hasBeenCollected = true;
            
            OnCollect();
        }

        protected virtual void OnCollect()
        {
            
        }
    }
}