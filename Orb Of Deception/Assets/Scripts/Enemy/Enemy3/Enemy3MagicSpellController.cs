using System;
using OrbOfDeception.Audio;
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
        //private PathFinder _pathFinder;
        private SoundsPlayer _soundsPlayer;

        private bool _isVanished;
        private Vector2 _direction;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            //_pathFinder = GetComponent<PathFinder>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        private void Start()
        {
            //_pathFinder.SetTarget(GameManager.Player.transform);
            //_pathFinder.StartFindingPath();
            _direction = GameManager.Player.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
        }

        private void FixedUpdate()
        {
            if (_isVanished) return;

            var pathfindingDirection = /*_pathFinder.GetCurrentPathDirection();*/ (GameManager.Player.transform.position - transform.position).normalized;
            _direction = Vector2.Lerp(_direction, pathfindingDirection, 0.55f);
            var force = _direction * (spellVelocity);
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
            
            //_pathFinder.StopFindingPath();
            
            vanishParticles.Play();
            idleParticles.Stop();
            idleParticles.Clear();
            
            _rigidbody.velocity = Vector2.zero;
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
            
            _soundsPlayer.Play("Explosion");
            
            // Destruir?
        }
    }
}
