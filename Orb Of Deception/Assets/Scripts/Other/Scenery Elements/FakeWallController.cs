using DG.Tweening;
using OrbOfDeception.Audio;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Enemy;
using OrbOfDeception.Player;
using UnityEngine;

namespace OrbOfDeception.Scenery_Elements
{
    public class FakeWallController : MonoBehaviour, IPlayerHittable
    {
        [SerializeField] private CameraLimits secretRoomCameraLimits;
        [SerializeField] private Collider2D fakeWallCollider;
        [SerializeField] private bool secretWallIsRight;

        private CameraLimits _previousCameraLimits;
        private SpriteMaterialController _wallMaterial;
        private SoundsPlayer _soundsPlayer;

        private float _dissolveValue;
        private bool _isHidden;
        
        private const float FadeDuration = 1f;
        
        private void Awake()
        {
            _wallMaterial = GetComponentInChildren<SpriteMaterialController>();
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        private void Update()
        {
            _wallMaterial.SetDissolve(_dissolveValue);
        }

        private void Hide()
        {
            _isHidden = true;
            
            _previousCameraLimits = GameManager.Camera.cameraLimits;
            GameManager.Camera.LerpToNewCameraLimits(secretRoomCameraLimits);

            fakeWallCollider.enabled = false;
            
            _soundsPlayer.Play("SecretDiscovered");
            
            DOTween.To(()=> _dissolveValue, x=> _dissolveValue = x, 1, FadeDuration);
        }

        private void Appear()
        {
            _isHidden = false;
            
            GameManager.Camera.LerpToNewCameraLimits(_previousCameraLimits);

            fakeWallCollider.enabled = true;
            
            DOTween.To(()=> _dissolveValue, x=> _dissolveValue = x, 0, FadeDuration);
        }
        
        public void OnPlayerHitEnter()
        {
            if (_isHidden) return;

            Hide();
        }

        public void OnPlayerHitExit()
        {
            if (!_isHidden) return;

            var isPlayerRightToWall = (GameManager.Player.transform.position.x - transform.position.x) >= 0;
            if ((!secretWallIsRight || isPlayerRightToWall) && (secretWallIsRight || !isPlayerRightToWall)) return;
            
            Appear();
        }
    }
}