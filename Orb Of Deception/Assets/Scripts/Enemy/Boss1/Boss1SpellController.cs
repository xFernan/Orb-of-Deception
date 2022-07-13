using OrbOfDeception.Audio;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class Boss1SpellController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem vanishParticles;
        [SerializeField] private ParticleSystem idleParticles;
        [SerializeField] private float spellVelocity;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;
        private SoundsPlayer _soundsPlayer;

        private bool _isVanished;
        private Vector3 _spellDirection;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        private void FixedUpdate()
        {
            if (_isVanished) return;
            
            var force = _spellDirection * (spellVelocity);
            _rigidbody.AddForce(force);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isVanished)
                return;
            
            var playerAreaDamage = other.GetComponent<PlayerAreaDamage>();

            if (playerAreaDamage != null)
            {
                playerAreaDamage.ReceiveDamage();
                VanishSpell();
                return;
            }
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
                VanishSpell();
        }

        private void VanishSpell()
        {
            _isVanished = true;
            
            vanishParticles.Play();
            idleParticles.Stop();
            idleParticles.Clear();
            
            _rigidbody.velocity = Vector2.zero;
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
            
            _soundsPlayer.Play("Explosion");
            
            // Destruir?
        }

        public void SetSpellDirection(Vector3 spellDirection)
        {
            _spellDirection = spellDirection;
        }
    }
}
