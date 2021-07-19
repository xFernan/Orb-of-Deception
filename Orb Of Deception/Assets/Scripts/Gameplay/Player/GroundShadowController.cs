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
        private Transform _spriteTransform;
        private Vector2 _originalScale;
        private static readonly int HideTriggerID = Animator.StringToHash("Hide");
        private const float TimeUntilStartHiding = 0.7f;

        #endregion
        
        #region Methods
        #region MonoBehaviour Methods
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.enabled = true;
            _spriteTransform = spriteRenderer.transform;
            
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
            
            var newScale = Mathf.Max(0, (maxHeight - distanceBetweenOriginAndFloor) / maxHeight);
            _spriteTransform.localScale = new Vector3(newScale * _originalScale.x, newScale * _originalScale.y, 1);
            
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

        private IEnumerator HideCoroutine()
        {
            yield return new WaitForSeconds(TimeUntilStartHiding);
            _animator.SetTrigger(HideTriggerID);
        }
        #endregion
        #endregion
    }
}
