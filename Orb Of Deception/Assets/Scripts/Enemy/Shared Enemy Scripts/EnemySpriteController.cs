using System;
using System.Collections.Generic;
using System.Linq;
using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemySpriteController : MonoBehaviour
    {
        [HideInInspector] public Color tintColor;
        [HideInInspector] public float tintOpacity;
        [HideInInspector] public float dissolve;
        [HideInInspector] public float opacity = 1;
        [HideInInspector] public float punishEffect;
        
        private readonly Color _blackColor = Color.black;
        private readonly Color _whiteColor = Color.white;
        private readonly Color _purpleColor = new Color(0.3f, 0, 0.7f);

        private SpriteMaterialController _materialController;
        
        #region Methods
        
        #region MonoBehaviour Methods
        
        private void Awake()
        {
            _materialController = GetComponent<SpriteMaterialController>();
            _materialController.SetUseBorderDuringDissolve(true);
        }

        private void Update()
        {
            _materialController.SetTintOpacity(tintOpacity);
            _materialController.SetDissolve(dissolve);
            _materialController.SetOpacity(opacity);
            _materialController.SetTintColor(tintColor);
            _materialController.SetPunishEffect(punishEffect);
        }
        
        #endregion
        
        #region Tint Methods
        
        private void SetTintColor(Color newTintColor)
        {
            tintColor = newTintColor;
        }
        
        public void SetTintToOppositeOrbColor()
        {
            var color = GetColorFromEntityColor(PlayerGroup.Orb.GetColor(), true);
            SetTintColor(color);
        }
        
        public void SetTintToOrbColor()
        {
            var color = GetColorFromEntityColor(PlayerGroup.Orb.GetColor(), false);
            SetTintColor(color);
        }
        
        public void SetTintToBlack()
        {
            SetTintColor(_blackColor);
        }

        public void SetTintToWhite()
        {
            SetTintColor(_whiteColor);
        }
        
        public void SetTintToPurple()
        {
            SetTintColor(_purpleColor);
        }
        
        private Color GetColorFromEntityColor(GameEntity.EntityColor entityColor, bool isOpposite)
        {
            return entityColor switch
            {
                GameEntity.EntityColor.Black => isOpposite ? _whiteColor : _blackColor,
                GameEntity.EntityColor.White => isOpposite ? _blackColor : _whiteColor,
                GameEntity.EntityColor.Other => _purpleColor,
                _ => throw new ArgumentOutOfRangeException(nameof(entityColor), entityColor, null)
            };
        }
        
        #endregion
        
        #endregion
    }
}
