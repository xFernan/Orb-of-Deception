using System;
using System.Linq;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerShadowController : MonoBehaviour
    {
        [Serializable]
        private class ShadowLevel
        {
            public float height;
            public Sprite sprite;
        }

        [SerializeField] private ShadowLevel[] shadowLevels = new ShadowLevel[4];
        [SerializeField] private float shadowOffsetIdle;
        [SerializeField] private float shadowOffsetMoving;

        private SpriteRenderer _spriteRenderer;
        private PlayerController _playerController;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerController = transform.parent.GetComponent<PlayerController>();
        }

        private void Update()
        {
            ApplyOffset();
            
            var playerPosition = _playerController.transform.position;
            
            var hit = Physics2D.Raycast(
                playerPosition,
                Vector2.down,
                shadowLevels.Last().height,
                LayerMask.GetMask(("Ground")));
            
            if (hit.collider == null)
            {
                _spriteRenderer.sprite = null;
                return;
            }

            transform.position = hit.point;
            
            var distanceBetweenPlayerAndFloor = Vector2.Distance(playerPosition, hit.point);
            
            int i = 0;
            bool shadowHeightDetected = false;
            while (i <= shadowLevels.Length && !shadowHeightDetected)
            {
                if (distanceBetweenPlayerAndFloor <= shadowLevels[i].height)
                {
                    SetShadowSprite(i);
                    shadowHeightDetected = true;
                }
                else
                {
                    i++;
                }
            }

            if (!shadowHeightDetected)
            {
                _spriteRenderer.sprite = null;
            }
        }

        private void ApplyOffset()
        {
            _spriteRenderer.transform.localPosition =
                transform.right * ((_playerController.IsMoving ? shadowOffsetMoving : shadowOffsetIdle) *
                                   (_playerController.Direction < 0 ? 1 : -1));
        }

        private void SetShadowSprite(int i)
        {
            _spriteRenderer.sprite = shadowLevels[i].sprite;
        }
        
        private void OnDrawGizmos()
        {
            var shadowRayColors = new Color[] { Color.red, Color.blue, Color.green, Color.magenta };
            var playerPosition = transform.parent.position;
            for (var i = shadowLevels.Length - 1; i >= 0 ; i--)
            {
                Gizmos.color = shadowRayColors[i];
                Gizmos.DrawLine(playerPosition, playerPosition + Vector3.down * shadowLevels[i].height);
            }
        }
    }
}
