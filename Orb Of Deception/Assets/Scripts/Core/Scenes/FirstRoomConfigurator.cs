using OrbOfDeception.Rooms;
using OrbOfDeception.UI.InGame_UI;
using UnityEngine;

namespace OrbOfDeception.Core.Scenes
{
    public class FirstRoomConfigurator : MonoBehaviour
    {

        private static bool _isFirstTime = true;

        private void Awake()
        {
            if (!_isFirstTime) return;
            
            SaveSystem.SetNewSpawn("Backstage", transform.position);
        }

        void Start()
        {
            if (!_isFirstTime) return;
            
            InGameMenuManager.Instance.titleAreaDisplayer.DisplayTitle("Backstage");
            GameManager.Camera.RePosition();

            _isFirstTime = false;
        }
        
    }
}