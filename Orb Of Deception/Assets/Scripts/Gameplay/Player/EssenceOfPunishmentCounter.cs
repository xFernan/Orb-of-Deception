using System;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class EssenceOfPunishmentCounter
    {
        public int essences = 0/*Provisional*/;
        public Action onEssenceAcquire;

        public void AcquireEssences(int essenceAmount)
        {
            essences = Mathf.Clamp(essences + essenceAmount, 0, 9999);
            onEssenceAcquire?.Invoke();
        }
    }
}