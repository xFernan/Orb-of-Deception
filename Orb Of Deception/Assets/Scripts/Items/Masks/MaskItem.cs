using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Items
{
    [CreateAssetMenu(fileName = "Mask", menuName = "Mask")]
    public class MaskItem : Item
    {
        public PlayerMaskController.MaskType maskType;
        public Sprite maskSprite;
        public Color particlesColor;
    }
}
