using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception
{
    public class OrbColorTutorialDisplayer : MonoBehaviour
    {

        [SerializeField] private int orbTutorialID;
        [SerializeField] private EnemyController enemyController;
        
        private ColliderEventTrigger _colliderEventTrigger;
        private HideableElement _hideableElement;

        private void Awake()
        {
            if (SaveSystem.HasTutorialBeenDisplayed(orbTutorialID)) gameObject.SetActive(false);
            
            _colliderEventTrigger = GetComponent<ColliderEventTrigger>();
            _hideableElement = GetComponent<HideableElement>();
        }

        private void Start()
        {
            enemyController.onDie += SaveTutorialDisplayed;
            _colliderEventTrigger.onTriggerEnter += ShowTutorial;
            _colliderEventTrigger.onTriggerExit += HideTutorial;
        }

        private void SaveTutorialDisplayed()
        {
            HideTutorial();
            SaveSystem.AddTutorialDisplayed(orbTutorialID);
        }

        private void ShowTutorial()
        {
            _hideableElement.Show();
        }

        private void HideTutorial()
        {
            _hideableElement.Hide();
        }
    }
}
