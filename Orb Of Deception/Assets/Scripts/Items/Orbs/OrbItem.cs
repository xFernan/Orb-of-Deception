using OrbOfDeception.Orb;
using UnityEngine;

namespace OrbOfDeception.Items
{
    [CreateAssetMenu(fileName = "Orb", menuName = "Orb")]
    public class OrbItem : Item
    {
        public OrbController.OrbType orbType;
    }
}