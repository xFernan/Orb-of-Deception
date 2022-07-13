using System;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Orb
{
    public class OrbSpriteController : MonoBehaviour
    {
        [SerializeField] private Sprite orbWhiteSprite;
        [SerializeField] private Sprite orbBlackSprite;
        
        public SpriteRenderer SpriteRenderer { get; private set; }

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetOrbSprite(GameEntity.EntityColor orbColor)
        {
            SpriteRenderer.sprite = orbColor switch
            {
                GameEntity.EntityColor.Black => orbBlackSprite,
                GameEntity.EntityColor.White => orbWhiteSprite,
                _ => SpriteRenderer.sprite
            };
        }

        /*private void LateUpdate() // Snap to player sprite coordinate system.
        {
            var playerSpritePosition = GameManager.Player.spriteObject.transform.position;
            transform.position = SnapToGrid.SnapVector3ToGrid(transform.parent.position - playerSpritePosition) +
                              playerSpritePosition + new Vector3(1/32.0f, 1/32.0f, 0);
        }*/
    }
}
