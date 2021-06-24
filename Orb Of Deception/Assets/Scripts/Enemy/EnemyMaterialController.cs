using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemyMaterialController : MonoBehaviour
    {
        
        [SerializeField] private GameObject maskObject;
        [SerializeField] private GameObject bodyObject;

        private EnemyController _enemyController;
        private GameEntity.EntityColor _enemyMaskColor;
        private Material _maskMaterial;
        private Material _bodyMaterial;

        [Range(0.0f, 1.0f)] public float maskOpacity;
        [Range(0.0f, 1.0f)] public float maskTintOpacity;
        [Range(0.0f, 1.0f)] public float maskDissolve;
        
        [Space]
        
        [Range(0.0f, 1.0f)] public float bodyOpacity;
        [Range(0.0f, 1.0f)] public float bodyTintOpacity;
        [Range(0.0f, 1.0f)] public float bodyDissolve;

        private static readonly int TintOpacity = Shader.PropertyToID("_TintOpacity");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
        private static readonly int TintIsWhite = Shader.PropertyToID("_TintIsWhite");

        private void Awake()
        {
            _enemyController = GetComponentInParent<EnemyController>();
            
            _maskMaterial = maskObject.GetComponent<SpriteRenderer>().material;
            _bodyMaterial = bodyObject.GetComponent<SpriteRenderer>().material;
        }

        private void Start()
        {
            // Se inicializan los valores por defecto.
            maskTintOpacity = 0;
            bodyTintOpacity = 0;
            maskOpacity = 1;
            bodyOpacity = 1;
            bodyDissolve = 1;
            maskDissolve = 1;
        }

        private void Update()
        {
            _maskMaterial.SetFloat(TintOpacity, maskTintOpacity);
            _maskMaterial.SetFloat(Opacity, maskOpacity);
            _maskMaterial.SetFloat(Dissolve, maskDissolve);
            
            
            _bodyMaterial.SetFloat(TintOpacity, bodyTintOpacity);
            _bodyMaterial.SetFloat(Opacity, bodyOpacity);
            _bodyMaterial.SetFloat(Dissolve, bodyDissolve);
        }

        private void SetSameColorAsMask()
        {
            _enemyMaskColor = _enemyController.GetMaskColor();
            _maskMaterial.SetInt(TintIsWhite, _enemyMaskColor == GameEntity.EntityColor.White ? 1 : 0);
            _bodyMaterial.SetInt(TintIsWhite, _enemyMaskColor == GameEntity.EntityColor.White ? 1 : 0);
        }
        
        private void SetColorAsOrb()
        {
            _enemyMaskColor = _enemyController.GetMaskColor();
            _maskMaterial.SetInt(TintIsWhite, _enemyMaskColor != GameEntity.EntityColor.White ? 1 : 0);
            _bodyMaterial.SetInt(TintIsWhite, _enemyMaskColor != GameEntity.EntityColor.White ? 1 : 0);
        }

        private void SetColorToWhite()
        {
            _maskMaterial.SetInt(TintIsWhite, 1);
            _bodyMaterial.SetInt(TintIsWhite, 1);
        }
        
        private void SetColorToBlack()
        {
            _maskMaterial.SetInt(TintIsWhite, 0);
            _bodyMaterial.SetInt(TintIsWhite, 0);
        }
        
    }
}
