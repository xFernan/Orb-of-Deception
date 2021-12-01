using System;

namespace OrbOfDeception.Gameplay.Player
{
    public class CollectibleCounter
    {
        public int collectibles = 0/*Provisional*/;
        public Action onCollectibleAcquire;

        public void AcquireCollectible()
        {
            collectibles++;
            onCollectibleAcquire?.Invoke();
        }
    }
}