using UnityEngine;

namespace Nanref.Core
{
    public class ParallaxEffectController : MonoBehaviour
    {
        [SerializeField] private float factor;
        private Transform _camera;
        private Vector3 _playerStartingPosition;
        private Vector3 _startingPosition;
        private SpriteRenderer _spriteRenderer;
    
        private void Awake()
        {
            _camera = Camera.main.transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _playerStartingPosition = _camera.position;
            _startingPosition = transform.position;
        }

        private void Update()
        {
            var newPosition = _startingPosition + (_playerStartingPosition - _camera.position) * factor;
            newPosition.y = _playerStartingPosition.y;
            transform.position = newPosition;
        }
    }
}
