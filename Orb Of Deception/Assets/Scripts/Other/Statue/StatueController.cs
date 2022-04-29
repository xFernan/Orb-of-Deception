using System;
using System.Collections;
using System.Collections.Generic;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core;
using OrbOfDeception.Rooms;
using OrbOfDeception.UI;
using OrbOfDeception.UI.InGame_UI;
using OrbOfDeception.UI.InGame_UI.Counter;
using OrbOfDeception.UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OrbOfDeception.Statue
{
    public class StatueController : MonoBehaviour
    {
        private class StatueRoomRenderer
        {
            private readonly Renderer _spriteRenderer;
            private readonly int _originalSortingLayer;
            private readonly int _originalSortingOrder;
            private readonly int _statueSortingLayerID;

            public StatueRoomRenderer(Renderer spriteRenderer, int statueSortingLayerID)
            {
                _spriteRenderer = spriteRenderer;
                _originalSortingLayer = _spriteRenderer.sortingLayerID;
                _originalSortingOrder = _spriteRenderer.sortingOrder;

                _statueSortingLayerID = statueSortingLayerID;
            }

            public void ReturnToOriginalLayer()
            {
                _spriteRenderer.sortingLayerID = _originalSortingLayer;
                _spriteRenderer.sortingOrder = _originalSortingOrder;
            }

            public void SetStatueLayer()
            {
                _spriteRenderer.sortingLayerName = "Statue";
                _spriteRenderer.sortingOrder = _statueSortingLayerID;
            }
        }

        [SerializeField] private SpriteRenderer statueShadowSpriteRenderer;
        [SerializeField] private SpriteRenderer statueIntactSpriteRenderer;
        [SerializeField] private SpriteRenderer statueBrokenSpriteRenderer;
        [SerializeField] private ParticleSystem backParticles;
        [SerializeField] private ParticleSystem frontParticles;
        
        [SerializeField] private ParticleSystem backgroundParticles;
        [SerializeField] private Animator whiteBackgroundAnimator;
        [SerializeField] private HideableElement interactIndicator;

        [Space]
        
        [SerializeField] private Transform playerRespawnTransform;
        
        private List<StatueRoomRenderer> _statueRoomSpriteRenderers;

        private CameraLimits _statueCameraLimits;
        
        private bool _isActive;
        
        private static readonly int IsVisible = Animator.StringToHash("IsVisible");

        private void Awake()
        {
            _statueCameraLimits = GetComponentInChildren<CameraLimits>();
            
            _statueRoomSpriteRenderers = new List<StatueRoomRenderer>();
        }

        private void Start()
        {
            _statueRoomSpriteRenderers.AddRange(
                new StatueRoomRenderer[]{
                    new StatueRoomRenderer(statueShadowSpriteRenderer, 0),
                    new StatueRoomRenderer(GameManager.Player.GroundShadowController.spriteRenderer, 0),
                    new StatueRoomRenderer(backParticles.GetComponent<Renderer>(), 1),
                    new StatueRoomRenderer(statueBrokenSpriteRenderer, 2),
                    new StatueRoomRenderer(statueIntactSpriteRenderer, 3),
                    new StatueRoomRenderer(frontParticles.GetComponent<Renderer>(), 4),
                    new StatueRoomRenderer(GameManager.Player.bodySpriteRenderer, 5),
                    new StatueRoomRenderer(GameManager.Player.maskSpriteRenderer, 6),
                    new StatueRoomRenderer(GameManager.Orb.OrbSpriteController.SpriteRenderer, 7)
                }
            );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _isActive = true;
            interactIndicator.Show();
            GameManager.Player.Interaction.onInteraction += Pray;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            interactIndicator.Hide();
            GameManager.Player.Interaction.onInteraction -= Pray;
            _isActive = false;
        }

        private void EnterStatueMenu()
        {
            StartCoroutine(EnterStatueMenuCoroutine());
        }

        private IEnumerator EnterStatueMenuCoroutine()
        {
            SaveSystem.SetNewSpawn(SceneManager.GetActiveScene().name, playerRespawnTransform.position);
            
            GameManager.Camera.ChangeCameraLimits(_statueCameraLimits);
            GameManager.Player.StatueMenuController.ApplyOffset();
            
            var player = GameManager.Player;
            player.StatueMenuController.InitializeMasksUnlocked();
            
            foreach (var statueRoomSpriteRenderer in _statueRoomSpriteRenderers)
            {
                statueRoomSpriteRenderer.SetStatueLayer();
            }
            
            GameManager.Orb.Hide();
            
            whiteBackgroundAnimator.SetBool(IsVisible, true);
            backgroundParticles.Play();

            InGameMenuManager.Instance.Open(GameManager.Player.StatueMenuController, false, false);
            
            player.HealthController.RecoverAll();
            
            yield return new WaitForSeconds(0.5f);
            
            InGameUIController.Instance.EssenceOfPunishmentDisplayer.ShowCounter();
            CollectibleDisplayer.Instance.ShowCounter();

            InGameMenuManager.Instance.onCloseMenu += ExitStatueMenu;
        }

        private void ExitStatueMenu()
        {
            StartCoroutine(ExitStatueMenuCoroutine());
        }

        private IEnumerator ExitStatueMenuCoroutine()
        {
            RoomManager.Instance.SetDefaultCameraLimits();
            
            whiteBackgroundAnimator.SetBool(IsVisible, false);
            backgroundParticles.Stop();
            GameManager.Player.KneelController.Stand();

            yield return new WaitForSeconds(0.55f);
            
            foreach (var statueRoomSpriteRenderer in _statueRoomSpriteRenderers)
            {
                statueRoomSpriteRenderer.ReturnToOriginalLayer();
            }
            
            GameManager.Orb.Appear();
            
            InGameUIController.Instance.EssenceOfPunishmentDisplayer.HideCounter();
            CollectibleDisplayer.Instance.HideCounter();

            InGameMenuManager.Instance.onCloseMenu -= ExitStatueMenu;
            GameManager.Player.isControlled = true;
        }
        
        private void Pray()
        {
            GameManager.Player.KneelController.Kneel();
            
            SaveSystem.ResetEnemies();
            SaveSystem.ResetDecorations();
            
            EnterStatueMenu();
        }

        private void OnDestroy()
        {
            InGameMenuManager.Instance.onCloseMenu -= ExitStatueMenu;
        }
        
    }
}
