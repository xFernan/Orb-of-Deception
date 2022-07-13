using OrbOfDeception.Audio;
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

        protected Animator animator;
        private Collider2D _collider;
        private SoundsPlayer _soundsPlayer;
        
        private LightController _lightController;
        private SpriteMaterialController _spriteMaterialController;
        
        private static readonly int HideTrigger = Animator.StringToHash("Hide");

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
            
            _lightController = GetComponentInChildren<LightController>();
            _spriteMaterialController = GetComponentInChildren<SpriteMaterialController>();
            _spriteMaterialController.SetTintColor(Color.white);
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
            _lightController.Hide();
            _collider.enabled = false;
            _hasBeenCollected = true;
            
            _soundsPlayer.Play("Collected");
            
            OnCollect();
        }

        protected virtual void OnCollect()
        {
            
        }
    }
}