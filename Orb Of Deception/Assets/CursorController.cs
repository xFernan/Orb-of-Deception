using System;
using OrbOfDeception.Gameplay.Player;
using OrbOfDeception.UI;
using UnityEngine;

namespace OrbOfDeception
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private float cursorVelocity;
        [SerializeField] private Texture2D cursorTexture;

        private SpriteRenderer _inGameCursorSprite;
        private ParticleSystem _particles;

        private void Awake()
        {
            _inGameCursorSprite = GetComponentInChildren<SpriteRenderer>();
            _particles = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            MenuManager.Instance.onCloseMenu += ForcePosition;
        }

        private void Update()
        {
            var menuIsOpen = MenuManager.Instance.isOpen;
            Cursor.visible = menuIsOpen;
            _inGameCursorSprite.enabled = !menuIsOpen;
            RePosition();
        }

        private void RePosition()
        {
            var targetPosition = GameManager.Camera.cameraComponent.ScreenToWorldPoint(Input.mousePosition);
            var newPosition = Vector3.Lerp(transform.position, targetPosition, cursorVelocity * Time.deltaTime);
            newPosition.z = 0;
            transform.position = newPosition;
        }

        private void ForcePosition()
        {
            _particles.Pause();
            var targetPosition = GameManager.Camera.cameraComponent.ScreenToWorldPoint(Input.mousePosition);
            transform.position = targetPosition;
            _particles.Play();
        }
    }
}
