using OrbOfDeception.Collectible;
using OrbOfDeception.UI;
using UnityEngine;

namespace OrbOfDeception
{
    public class CollectibleItemController : CollectibleController
    {
        [SerializeField] private Item item;
        
        protected override void CollectEffect()
        {
            ItemObtainedMenu.Instance.ShowItem(item);
            // Añadir en la data de la partida que se ha desbloqueado el item en cuestión.
        }
    }
}