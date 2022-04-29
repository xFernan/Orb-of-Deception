namespace OrbOfDeception.UI.InGame_UI.Counter
{
    public class EssenceOfPunishmentDisplayer : UIValueDisplayer
    {
        private void Start()
        {
            GameManager.Player.EssenceOfPunishmentCounter.onEssenceAcquire += ShowCounterBriefly;
        }
        protected override float targetValueToShow => GameManager.Player.EssenceOfPunishmentCounter.essences;
    }
}