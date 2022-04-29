using System;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Enemy.Enemy3
{
    public class Enemy3MagicSpellController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem vanishParticles;
        [SerializeField] private ParticleSystem idleParticles;
        [SerializeField] private float spellVelocity;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;

        private bool _isVanished;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            if (_isVanished) return;
            
            var forceDirection = (GameManager.Player.transform.position - transform.position).normalized;
            var force = forceDirection * (spellVelocity);
            _rigidbody.AddForce(force);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isVanished)
                return;
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
                VanishSpell();
            
            var playerAreaDamage = other.GetComponent<PlayerAreaDamage>();

            if (playerAreaDamage == null) return;
            
            playerAreaDamage.ReceiveDamage();
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
            
            // Destruir?
        }
    }
}
