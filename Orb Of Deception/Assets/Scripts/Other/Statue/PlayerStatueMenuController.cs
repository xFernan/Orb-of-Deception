using OrbOfDeception.Items;
using OrbOfDeception.Rooms;
using OrbOfDeception.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.Statue
{
    public class PlayerStatueMenuController : AnimatedMenuController
    {
        [SerializeField] private float offsetX;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textDescription;
        [SerializeField] private Image maskImage;
        [SerializeField] private RectTransform maskSlot;

        private MaskItem[] _masksUnlocked;

        private int _currentMaskID;

        private Animator _animator;
        private RectTransform _rectTransform;
        
        private static readonly int ChangeMask = Animator.StringToHash("ChangeMask");

        protected override void Awake()
        {
            base.Awake();
            
            _animator = GetComponent<Animator>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void InitializeMasksUnlocked()
        {
            _masksUnlocked = new MaskItem[SaveSystem.MasksUnlocked.Count];

            var unlockedMaskID = 0;
            foreach (var maskInfo in GameManager.Player.MaskController.masksInfo)
            {
                if (!SaveSystem.MasksUnlocked.Contains(maskInfo.maskType))
                    continue;
                
                _masksUnlocked[unlockedMaskID] = maskInfo;
                if (maskInfo.maskType == SaveSystem.currentMaskType)
                    _currentMaskID = unlockedMaskID;
                unlockedMaskID++;
            }
            
            UpdateMaskInfo();
        }

        private void UpdateMaskInfo()
        {
            var currentMaskInfo = _masksUnlocked[_currentMaskID];
            textName.text = currentMaskInfo.itemName;
            textName.color = currentMaskInfo.nameColor;
            textDescription.text = currentMaskInfo.itemDescription;
            maskImage.sprite = currentMaskInfo.itemUISprite;
        }

        public void EquipNextMask()
        {
            _currentMaskID++;
            
            EquipMask(_currentMaskID);
        }
        
        public void EquipPreviousMask()
        {
            _currentMaskID--;
            if (_currentMaskID < 0)
                _currentMaskID += _masksUnlocked.Length;
            
            EquipMask(_currentMaskID);
        }

        private void EquipMask(int newMaskID)
        {
            if (_masksUnlocked.Length <= 1)
            {
                // Play "Wrong" Sound Effect.
                return;
            }
            
            _currentMaskID = (newMaskID) % _masksUnlocked.Length;
            SaveSystem.currentMaskType = _masksUnlocked[_currentMaskID].maskType;
            _animator.SetTrigger(ChangeMask);
            GameManager.Player.MaskController.PlayMaskChangeAnimation();
        }

        public void ApplyOffset()
        {
            var flip = GameManager.Player.SpriteDirectionController.flip;
            
            var anchoredPosition = _rectTransform.anchoredPosition;
            anchoredPosition.x = flip * offsetX;
            _rectTransform.anchoredPosition = anchoredPosition;

            var localScale = maskSlot.localScale;
            localScale.x = flip;
            maskSlot.localScale = localScale;
        }
    }
}
