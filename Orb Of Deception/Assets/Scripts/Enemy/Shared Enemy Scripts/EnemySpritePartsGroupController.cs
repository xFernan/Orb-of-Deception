using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemySpritePartsGroupController : MonoBehaviour
    {
        #region Variables
        
        private List<SpriteMaterialController> _parts;

        [HideInInspector] public Color tintColor;
        [HideInInspector] public float tintOpacity;
        [HideInInspector] public float dissolve;
        [HideInInspector] public float opacity = 1;
        [HideInInspector] public float punishEffect;
        
        #endregion

        #region Methods

        #region MonoBehaviour Methods
        
        private void Awake()
        {
            _parts = GetComponentsInChildren<SpriteMaterialController>().ToList();
        }

        private void Update()
        {
            foreach (var part in _parts)
            {
                part.SetTintOpacity(tintOpacity);
                part.SetDissolve(dissolve);
                part.SetOpacity(opacity);
                part.SetTintColor(tintColor);
                part.SetPunishEffect(punishEffect);
            }
        }
        
        #endregion
        
        public void SetTintColor(Color newTintColor)
        {
            tintColor = newTintColor;
        }
        
        #endregion
    }
}
