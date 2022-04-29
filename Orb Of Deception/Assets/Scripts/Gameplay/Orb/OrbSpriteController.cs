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
        
    }
}
