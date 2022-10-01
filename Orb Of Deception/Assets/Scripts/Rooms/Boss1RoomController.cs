using System.Collections;
using OrbOfDeception.Audio;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core;
using OrbOfDeception.Enemy.Boss1;
using OrbOfDeception.UI.InGame_UI;
using UnityEngine;

namespace OrbOfDeception.Rooms
{
    public class Boss1RoomController : MonoBehaviour
    {
        [SerializeField] private float delayToInitBossRoom = 1;
        [SerializeField] private Boss1Controller bossController;
        [SerializeField] private Collider2D[] invisibleWalls;
        [SerializeField] private Boss1EnemySpawner[] boss1EnemySpawners;

        private Animator _animator;
        private ColliderEventTrigger _eventTrigger;
        private CameraLimits _bossRoomCameraLimits;
        
        private static readonly int Appear = Animator.StringToHash("Appear");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _eventTrigger = GetComponentInChildren<ColliderEventTrigger>();
            _bossRoomCameraLimits = GetComponentInChildren<CameraLimits>();
        }

        private void Start()
        {
            _eventTrigger.onTriggerEnter += InitBossRoom;
            
            foreach (var wall in invisibleWalls)
            {
                wall.enabled = false;
            }
        }

        private void InitBossRoom()
        {
            StartCoroutine(InitBossRoomCoroutine());
        }

        private IEnumerator InitBossRoomCoroutine()
        {
            GameManager.Player.isControlled = false;
            GameManager.Camera.LerpToNewCameraLimits(_bossRoomCameraLimits);

            yield return new WaitForSeconds(delayToInitBossRoom);
            
            _animator.SetTrigger(Appear);
            foreach (var wall in invisibleWalls)
            {
                wall.enabled = true;
            }
            
            MusicManager.Instance.PlayMusic("Boss1", false);
            
            yield return new WaitForSeconds(1.5f);
            
            bossController.Appear();
            bossController.onDie += EndBossRoom;
            InGameMenuManager.Instance.titleBossDisplayer.DisplayTitle("-Sin-");
            
            yield return new WaitForSeconds(1f);
            
            GameManager.Player.isControlled = true;
            bossController.InitBattle();
            foreach (var bossRoomSpawner in boss1EnemySpawners)
            {
                bossRoomSpawner.StartSpawning();
            }
        }

        private void EndBossRoom()
        {
            StartCoroutine(EndBossRoomCoroutine());
        }

        private IEnumerator EndBossRoomCoroutine()
        {
            foreach (var bossRoomSpawner in boss1EnemySpawners)
            {
                bossRoomSpawner.StopSpawning();
            }
            
            MusicManager.Instance.StopMusic();
            
            yield return new WaitForSeconds(4.5f);
            
            ShowEndDemoScreen();
        }

        public void ShowEndDemoScreen()
        {
            InGameMenuManager.Instance.endDemoMenuController.Open();
        }
    }
}
