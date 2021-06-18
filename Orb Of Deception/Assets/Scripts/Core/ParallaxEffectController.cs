using UnityEngine;

namespace OrbOfDeception.Core
{
    public class ParallaxEffectController : MonoBehaviour
    {
        [SerializeField] private float factorX;
        [SerializeField] private float factorY;
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
            var parallaxVariation = _playerStartingPosition - _camera.position;
            parallaxVariation.x *= factorX;
            parallaxVariation.y *= factorY;
            var newPosition = _startingPosition + parallaxVariation;
            transform.position = newPosition;
        }
    }
}
