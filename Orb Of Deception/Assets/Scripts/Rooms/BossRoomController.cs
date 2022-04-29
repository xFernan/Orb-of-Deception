using System.Collections;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core;
using OrbOfDeception.UI.Menu;
using UnityEngine;

namespace OrbOfDeception.Rooms
{
    public class BossRoomController : MonoBehaviour
    {
        [SerializeField] private float delayToInitBossRoom = 1;
        [SerializeField] private GameObject[] platformsToEnable;

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
            DisablePlatforms();
            _eventTrigger.onTrigger += InitBossRoom;
        }

        private void EnablePlatforms()
        {
            foreach (var platform in platformsToEnable)
            {
                platform.SetActive(true);
            }
        }
        
        private void DisablePlatforms()
        {
            foreach (var platform in platformsToEnable)
            {
                platform.SetActive(false);
            }
        }

        private void InitBossRoom()
        {
            StartCoroutine(InitBossRoomCoroutine());
        }

        private IEnumerator InitBossRoomCoroutine()
        {
            GameManager.Player.isControlled = false;
            GameManager.Camera.cameraLimits = _bossRoomCameraLimits;

            yield return new WaitForSeconds(delayToInitBossRoom);
            
            _animator.SetTrigger(Appear);

            yield return new WaitForSeconds(1.5f);

            InGameMenuManager.Instance.titleDisplayer.DisplayTitle("Sin");
            GameManager.Player.isControlled = true;
            
            // Provisional lo siguiente;
            yield return new WaitForSeconds(6);
            InGameMenuManager.Instance.endDemoMenuController.Open();
        }
    }
}
