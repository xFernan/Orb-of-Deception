using System.Collections.Generic;
using OrbOfDeception.Enemy;
using OrbOfDeception.Player;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI.InGame_UI.Gems
{
    public class UIGemController : MonoBehaviour
    {
        [System.Serializable]
        private class Gem
        {
            public PlayerMaskController.MaskType maskType;
            public Sprite gemSprite;
        }

        [SerializeField] private Gem[] gems;
        
        private Image _image;
        
        public ImageMaterialController ImageMaterialController { get; private set; }

        private Dictionary<PlayerMaskController.MaskType, Sprite> _gemSpritesDictionary;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            ImageMaterialController = GetComponent<ImageMaterialController>();
        }

        private void Start()
        {
            _gemSpritesDictionary = new Dictionary<PlayerMaskController.MaskType, Sprite>();
            foreach (var gem in gems)
            {
                _gemSpritesDictionary.Add(gem.maskType, gem.gemSprite);
            }
        }

        public void UpdateGemSprite(PlayerMaskController.MaskType maskType)
        {
            _image.sprite = _gemSpritesDictionary[maskType];
        }
    }
}
