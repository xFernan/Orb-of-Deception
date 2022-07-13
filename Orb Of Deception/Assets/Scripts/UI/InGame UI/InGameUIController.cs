using OrbOfDeception.UI.InGame_UI.Counter;
using OrbOfDeception.UI.InGame_UI.Gems;
using OrbOfDeception.UI.InGame_UI.Health_Bar;
using UnityEngine;

namespace OrbOfDeception.UI.InGame_UI
{
    public class InGameUIController : MonoBehaviour
    {
        private static InGameUIController _instance;

        public static InGameUIController Instance => _instance;
        
        private Canvas _canvas;
        public EssenceOfPunishmentDisplayer EssenceOfPunishmentDisplayer { get; private set; }
        public MedallionOrbController MedallionOrbController { get; private set; }
        public PlayerHealthBarController PlayerHealthBarController { get; private set; }
        public CollectibleDisplayer CollectibleDisplayer { get; private set; }
        public UIGemsController UIGemsController { get; private set; }

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            
            DontDestroyOnLoad(this);
            _instance = this;

            _canvas = GetComponent<Canvas>();
            EssenceOfPunishmentDisplayer = GetComponentInChildren<EssenceOfPunishmentDisplayer>();
            MedallionOrbController = GetComponentInChildren<MedallionOrbController>();
            PlayerHealthBarController = GetComponentInChildren<PlayerHealthBarController>();
            CollectibleDisplayer = GetComponentInChildren<CollectibleDisplayer>();
            UIGemsController = GetComponentInChildren<UIGemsController>();
        }
    }
}
