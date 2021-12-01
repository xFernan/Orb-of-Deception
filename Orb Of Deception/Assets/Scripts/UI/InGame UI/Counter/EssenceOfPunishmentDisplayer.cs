using OrbOfDeception.Gameplay.Player;

namespace OrbOfDeception.UI.Essence_of_Punishment_Counter
{
    public class EssenceOfPunishmentDisplayer : UIValueDisplayer
    {
        private void Start()
        {
            GameManager.Player.EssenceOfPunishmentCounter.onEssenceAcquire += ShowCounter;
        }
        protected override float targetValueToShow => GameManager.Player.EssenceOfPunishmentCounter.essences;
    }
}