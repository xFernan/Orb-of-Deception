using OrbOfDeception.Rooms;
using OrbOfDeception.UI.InGame_UI;
using TMPro;
using UnityEngine;

namespace OrbOfDeception.UI
{
    public class EndDemoMenuController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI deathsText;
        [SerializeField] private TextMeshProUGUI hitsText;
        [SerializeField] private TextMeshProUGUI collectiblesText;
        [SerializeField] private TextMeshProUGUI essencesText;
        
        private AnimatedMenuController _animatedMenuController;
        
        private void Awake()
        {
            _animatedMenuController = GetComponentInChildren<AnimatedMenuController>();
        }

        public void Open()
        {
            var timePlayed = Mathf.RoundToInt(SaveSystem.TimePlayed);
            var minutes = timePlayed / 60;
            var seconds = timePlayed % 60;

            var timeWritten = "";
            if (minutes / 10 == 0)
                timeWritten += "0";
            timeWritten += minutes + ":";
            if (seconds / 10 == 0)
            {
                timeWritten += "0";
            }
            timeWritten += seconds;
            
            timeText.text = timeWritten;
            deathsText.text = SaveSystem.GetNumberOfDeaths() + "";
            hitsText.text = SaveSystem.GetNumberOfHits() + "";
            
            collectiblesText.text = SaveSystem.GetCollectiblesAcquiredAmount() + "/4";

            essencesText.text = "x" + GameManager.Player.EssenceOfPunishmentCounter.GetAcquiredEssences();
            
            InGameMenuManager.Instance.Open(_animatedMenuController);
        }
    }
}
