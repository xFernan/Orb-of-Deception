using OrbOfDeception.Enemy;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerMaskWhiteFadeController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer _playerMaskSpriteRenderer;
        private SpriteMaterialController _spriteMaterialController;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerMaskSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
            _spriteMaterialController = GetComponent<SpriteMaterialController>();
        }

        private void Start()
        {
            _spriteMaterialController.SetTintColor(Color.white);
            _spriteMaterialController.SetTintOpacity(1);
        }

        private void Update()
        {
            _spriteRenderer.sprite = _playerMaskSpriteRenderer.sprite;
            _spriteRenderer.sortingLayerID = _playerMaskSpriteRenderer.sortingLayerID;
            _spriteRenderer.sortingOrder = _playerMaskSpriteRenderer.sortingOrder + 1;
        }
    }
}
