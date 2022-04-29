using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public class FallingPlatform : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float particlesRateOnFalling = 20;
        [SerializeField] private float timePreparingToFall = 1f;
        [SerializeField] private float timeUntilRecovering = 2f;

        [Space]
        
        [SerializeField] private ParticleSystem fallingParticles;
        
        private bool _isFalling;
        private Bounds _platformBounds;
        
        private Animator _animator;
        private BoxCollider2D _collider;
        private SpriteRenderer _spriteRenderer;
        
        private static readonly int FallTrigger = Animator.StringToHash("Fall");
        private static readonly int RecoverTrigger = Animator.StringToHash("Recover");

        #endregion
        
        #region Methods
        
        #region MonoBehaviour Methods
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            AdjustPlatformSize();
        }

        private void AdjustPlatformSize()
        {
            var width = Mathf.RoundToInt(_spriteRenderer.size.x);
            _spriteRenderer.size = new Vector2(width, 1);

            var fallingParticlesShape = fallingParticles.shape;
            var particlesShapeScale = fallingParticlesShape.scale;
            particlesShapeScale.x = width;
            fallingParticlesShape.scale = particlesShapeScale;

            var fallingParticlesEmission = fallingParticles.emission;
            fallingParticlesEmission.rateOverTime = particlesRateOnFalling * width;

            var size = _collider.size;
            size = new Vector2(width, size.y);
            _collider.size = size;

            _platformBounds = new Bounds {size = size, center = transform.position + (Vector3) _collider.offset};
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isFalling)
                return;
            
            var playerObject = GameManager.Player.gameObject;
            if (other.gameObject == playerObject && other.GetContact(0).normal == Vector2.down)
            {
                Fall();
            }
        }
        
        #endregion
        
        #region FallingPlatform Methods
        private void Fall()
        {
            StartCoroutine(FallCoroutine());
        }

        private IEnumerator FallCoroutine()
        {
            _isFalling = true;
            
            fallingParticles.Play();
            _animator.enabled = false;
            _spriteRenderer.transform.DOShakePosition(0.2f, 0.15f, 20);
            
            yield return new WaitForSeconds(timePreparingToFall);
            
            _collider.enabled = false;
            if (AstarPath.active != null)
                AstarPath.active.UpdateGraphs(_platformBounds);
            _animator.enabled = true;
            _animator.SetTrigger(FallTrigger);
            
            yield return new WaitForSeconds(timeUntilRecovering);

            _animator.SetTrigger(RecoverTrigger);
        }

        private void OnRecoverEnds()
        {
            _collider.enabled = true;
            if (AstarPath.active != null)
                AstarPath.active.UpdateGraphs(_platformBounds);
            
            _isFalling = false;
        }
        
        #endregion
        
        #endregion
    }
}
