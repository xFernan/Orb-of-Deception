using UnityEngine;

namespace OrbOfDeception.Collectible
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
    }
}