using System.Collections;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class GroundShadowController : MonoBehaviour
    {
        #region Variables
        
        [SerializeField] private Transform shadowOrigin;
        [SerializeField] private float maxHeight;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Transform _spriteTransform;
        private Vector2 _originalScale;
        
        private const float TimeUntilStartHiding = 0.7f;
        private const float MaxShadowOpacity = 0.25f;
        
        private static readonly int HideTrigger = Animator.StringToHash("Hide");
        private static readonly int AppearTrigger = Animator.StringToHash("Appear");
        
        #endregion
        
        #region Methods
        
        #region MonoBehaviour Methods
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.enabled = true;
            _spriteTransform = _spriteRenderer.transform;
            
            _originalScale = _spriteTransform.localScale;
        }

        private void Update()
        {
            var playerFootPosition = shadowOrigin.position;
            
            var hit = Physics2D.Raycast(
                playerFootPosition,
                Vector2.down,
                maxHeight,
                LayerMask.GetMask(("Ground")));

            if (hit.collider == null)
            {
                _spriteTransform.localScale = Vector3.zero;
                return;
            }
            
            transform.position = hit.point;
            
            var distanceBetweenOriginAndFloor = Vector2.Distance(playerFootPosition, hit.point);
            
            var distanceFactor = Mathf.Max(0, (maxHeight - distanceBetweenOriginAndFloor) / maxHeight);
            var newScale = new Vector3(_originalScale.x, _originalScale.y, 1) * distanceFactor;
            _spriteTransform.localScale = newScale;
            
            var newColor = _spriteRenderer.color;
            newColor.a = distanceFactor * MaxShadowOpacity;
            _spriteRenderer.color = newColor;
        }
        
        private void OnDrawGizmos()
        {
            var shadowOriginPosition = shadowOrigin.position;
            Gizmos.color = Color.black;
            Gizmos.DrawLine(shadowOriginPosition, shadowOriginPosition + Vector3.down * maxHeight);
        }
        
        #endregion
        
        #region Ground Shadow Controller Methods
        
        public void Hide()
        {
            StartCoroutine(HideCoroutine());
        }

        public void Appear()
        {
            _animator.SetTrigger(AppearTrigger);
        }
        
        private IEnumerator HideCoroutine()
        {
            yield return new WaitForSeconds(TimeUntilStartHiding);
            _animator.SetTrigger(HideTrigger);
        }
        
        #endregion
        
        #endregion
    }
}
