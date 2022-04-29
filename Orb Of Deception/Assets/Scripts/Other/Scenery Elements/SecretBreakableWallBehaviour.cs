using System.Collections.Generic;
using DG.Tweening;
using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using OrbOfDeception.Orb;
using OrbOfDeception.Rooms;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace OrbOfDeception.Scenery_Elements
{
    public class SecretBreakableWallBehaviour : MonoBehaviour, IOrbCollisionable
    {
        [SerializeField] private int secretWallID;
        [SerializeField] private GameObject secretWallFront;
        [SerializeField] private bool hasFakeWall;
        [ShowIf("hasFakeWall", true)] [SerializeField] private GameObject fakeWall;
        [ShowIf("hasFakeWall", true)] [SerializeField] private List<int> roomChangerIdsToShowFakeWall;

        private SpriteMaterialController _secretWallFrontMaterial;
        private SpriteMaterialController _wallMaterial;
        private TilemapCollider2D _wallCollider;

        private float _dissolveValue;
        private bool _isOpen;

        private const float FadeDuration = 1f;
        
        private void Awake()
        {
            _secretWallFrontMaterial = secretWallFront.GetComponent<SpriteMaterialController>();
            _wallCollider = GetComponent<TilemapCollider2D>();
            _wallMaterial = GetComponent<SpriteMaterialController>();
        }

        private void Start()
        {
            if (SaveSystem.IsSecretWallDiscovered(secretWallID))
                HideBoth();
            else
            {
                if (roomChangerIdsToShowFakeWall.Contains(RoomManager.targetRoomChangerID))
                {
                    EnableFakeWall();
                }
                else
                {
                    EnableBreakableWall();
                }
            }
        }

        private void Update()
        {
            _wallMaterial.SetDissolve(_dissolveValue);
            _secretWallFrontMaterial.SetDissolve(_dissolveValue);
        }

        public void OnOrbCollisionEnter()
        {
            if (_isOpen) return;
            Open();
        }

        private void Open()
        {
            SaveSystem.AddSecretWallDiscovered(secretWallID);
            
            DOTween.To(()=> _dissolveValue, x=> _dissolveValue = x, 1, FadeDuration);
            _wallCollider.enabled = false;

            AstarPath.active.Scan();
            
            _isOpen = true;
        }

        private void EnableFakeWall()
        {
            if (fakeWall != null)
                fakeWall.SetActive(true);
            _wallCollider.enabled = false;
            AstarPath.active.Scan();
            
            gameObject.SetActive(false);
        }
        
        private void EnableBreakableWall()
        {
            _secretWallFrontMaterial.gameObject.SetActive(true);
            if (fakeWall != null)
                fakeWall.SetActive(false);
            AstarPath.active.Scan();
        }

        private void HideBoth()
        {
            if (fakeWall != null)
                fakeWall.SetActive(false);
            _wallCollider.enabled = false;
            AstarPath.active.Scan();
            
            gameObject.SetActive(false);
        }
    }
}
