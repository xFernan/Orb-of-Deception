using UnityEngine;

namespace OrbOfDeception.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public string itemDescription;
        public Sprite itemUISprite;
        public Color nameColor;
    }
}