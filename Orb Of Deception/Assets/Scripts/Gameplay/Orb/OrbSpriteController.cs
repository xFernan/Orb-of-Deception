using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Orb
{
    public class OrbSpriteController : MonoBehaviour
    {
        [SerializeField] private Sprite orbWhiteSprite;
        [SerializeField] private Sprite orbBlackSprite;
        
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetOrbSprite(GameEntity.EntityColor orbColor)
        {
            _spriteRenderer.sprite = orbColor switch
            {
                GameEntity.EntityColor.Black => orbBlackSprite,
                GameEntity.EntityColor.White => orbWhiteSprite,
                _ => _spriteRenderer.sprite
            };
        }
        
    }
}
