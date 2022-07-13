using OrbOfDeception.Player;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI.Gems
{
    public class UIGemsController : MonoBehaviour
    {
        [HideInInspector] public float tintOpacity;
        [SerializeField] private UIGemController gem1Controller;
        [SerializeField] private UIGemController gem2Controller;

        private Animator _animator;

        private PlayerMaskController.MaskType _currentMaskType;
        
        private static readonly int ChangeColor = Animator.StringToHash("ChangeColor");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            gem1Controller.ImageMaterialController.SetTintOpacity(tintOpacity);
            gem2Controller.ImageMaterialController.SetTintOpacity(tintOpacity);
        }

        public void UpdateGems()
        {
            _animator.SetTrigger(ChangeColor);
            _currentMaskType = SaveSystem.currentMaskType;
        }

        private void UpdateGemsSprite()
        {
            gem1Controller.UpdateGemSprite(_currentMaskType);
            gem2Controller.UpdateGemSprite(_currentMaskType);
        }

    }
}
