using System;
using System.Collections;
using OrbOfDeception.Collectible;
using OrbOfDeception.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception
{
    public class ItemObtainedMenu : MonoBehaviour
    {
        private static ItemObtainedMenu _instance;

        public static ItemObtainedMenu Instance => _instance;

        [SerializeField] private float showMenuDelay = 1;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private Image itemImage;

        private void Awake()
        {
            _instance = this;
        }

        public void ShowItem(Item itemToShow)
        {
            StartCoroutine(ShowItemCoroutine(itemToShow));
        }

        private IEnumerator ShowItemCoroutine(Item itemToShow)
        {
            yield return new WaitForSeconds(showMenuDelay);
            UpdateItemInfo(itemToShow);
            var itemObtainedSubmenu = GetComponent<SubMenuController>();
            MenuManager.Instance.Open(itemObtainedSubmenu);
        }
        
        private void UpdateItemInfo(Item itemToDisplay)
        {
            itemNameText.text = itemToDisplay.itemName;
            itemDescriptionText.text = itemToDisplay.itemDescription;
            itemImage.sprite = itemToDisplay.itemSprite;
        }
    }
}
