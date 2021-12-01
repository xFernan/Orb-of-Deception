using OrbOfDeception.Gameplay.Player;

namespace OrbOfDeception
{
    public class CollectibleTriangleController : CollectibleController
    {
        protected override void CollectEffect()
        {
            GameManager.Player.CollectibleCounter.AcquireCollectible();
        }
    }
}