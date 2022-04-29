using OrbOfDeception.Core;
using OrbOfDeception.Orb;
using OrbOfDeception.Rooms;
using UnityEngine;
using UnityEngine.Events;

namespace OrbOfDeception.Switch
{
    public class SwitchController : MonoBehaviour, IOrbHittable
    {
        [SerializeField] private int switchID;
        
        [Space]
        
        [SerializeField] private Sprite whiteNotActivatedSprite;
        [SerializeField] private Sprite whiteActivatedSprite;
        [SerializeField] private Sprite blackNotActivatedSprite;
        [SerializeField] private Sprite blackActivatedSprite;
        
        [Space]
        
        [SerializeField] private GameEntity.EntityColor color;
        [SerializeField] private ParticleSystem idleParticles;
        [SerializeField] private ParticleSystem activateParticles;
        
        [Space]
        
        [SerializeField] private UnityEvent onActivate;

        private bool _isActivated;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            var isBlack = color == GameEntity.EntityColor.Black;
            _spriteRenderer.sprite =
                isBlack ? blackNotActivatedSprite : whiteNotActivatedSprite;
            var main = idleParticles.main;
            main.startColor = isBlack ? Color.black : Color.white;
            main = activateParticles.main;
            main.startColor = isBlack ? Color.white : Color.black;
            idleParticles.Clear();
            idleParticles.Play();
        }

        private void Start()
        {
            var hasBeenActivated = SaveSystem.IsSwitchActivated(switchID);
            if (!hasBeenActivated) return;
            
            _isActivated = true;
            _spriteRenderer.sprite =
                color == GameEntity.EntityColor.Black ? blackActivatedSprite : whiteActivatedSprite;
            idleParticles.Clear();
            idleParticles.Stop();
        }

        private void Activate()
        {
            if (_isActivated)
                return;
            
            _isActivated = true;
            _spriteRenderer.sprite =
                color == GameEntity.EntityColor.Black ? blackActivatedSprite : whiteActivatedSprite;
            idleParticles.Stop();
            activateParticles.Play();
            onActivate?.Invoke();
            
            SaveSystem.AddSwitchActivated(switchID);
        }

        public void OnOrbHitEnter(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0)
        {
            if (damageColor != color)
                Activate();
        }

        public bool IsActivated()
        {
            return _isActivated;
        }
    }
}
