using System.Collections;
using OrbOfDeception.Audio;
using OrbOfDeception.Items;
using OrbOfDeception.UI.InGame_UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI
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
            var itemObtainedSubmenu = GetComponent<MenuController>();
            InGameMenuManager.Instance.Open(itemObtainedSubmenu);
        }
        
        private void UpdateItemInfo(Item itemToDisplay)
        {
            itemNameText.text = itemToDisplay.itemName;
            itemNameText.color = itemToDisplay.nameColor;
            itemDescriptionText.text = itemToDisplay.itemDescription;
            itemImage.sprite = itemToDisplay.itemUISprite;
        }
    }
}
