using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyMaskController : MonoBehaviour
    {
        [SerializeField] private Sprite whiteMask;
        [SerializeField] private Sprite blackMask;
        
        private EnemyController _enemyController;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _enemyController = GetComponentInParent<EnemyController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _enemyController.BaseParameters.onMaskColorChange += UpdateMask;
        }

        private void OnDisable()
        {
            _enemyController.BaseParameters.onMaskColorChange -= UpdateMask;
        }

        private void UpdateMask()
        {
            _spriteRenderer.sprite =
                _enemyController.BaseParameters.MaskColor == GameEntity.EntityColor.White
                ? whiteMask
                : blackMask;
        }
    }
}
