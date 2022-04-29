using OrbOfDeception.UI.Menu;
using UnityEngine;

namespace OrbOfDeception.UI
{
    public class CursorController : MonoBehaviour
    {
        private static CursorController _instance;

        public static CursorController Instance => _instance;
        
        [SerializeField] private float cursorVelocity;

        private SpriteRenderer _inGameCursorSprite;
        private ParticleSystem _particles;

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            
            DontDestroyOnLoad(this);
            _instance = this;
            
            _inGameCursorSprite = GetComponentInChildren<SpriteRenderer>();
            _particles = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            InGameMenuManager.Instance.onCloseMenu += ForcePosition;
        }

        private void Update()
        {
            RePosition();
            
            var menuIsOpen = InGameMenuManager.Instance.isOpen;
            if (Cursor.visible == menuIsOpen)
                return;
            
            Cursor.visible = menuIsOpen;
            
            if (menuIsOpen)
                _particles.Stop();
            else
                _particles.Play();
            
            _inGameCursorSprite.enabled = !menuIsOpen;
        }

        /*private void LateUpdate()
        {
            var position = transform.position;
            position.x = ((float) Mathf.RoundToInt(position.x * 16)) / 16;
            position.y = ((float) Mathf.RoundToInt(position.y * 16)) / 16;
            transform.position = position;
        }*/

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
