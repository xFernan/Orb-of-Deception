using System;
using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class EnemySpriteTintController : MonoBehaviour
    {
        [SerializeField] private EnemySpritePartsGroupController masksGroup;
        [SerializeField] private EnemySpritePartsGroupController bodyGroup;
        
        private readonly Color _blackColor = Color.black;
        private readonly Color _whiteColor = Color.white;
        private readonly Color _purpleColor = new Color(0.3f, 0, 0.7f);

        private void SetTintColor(Color newTintColor)
        {
            masksGroup.SetTintColor(newTintColor);
            bodyGroup.SetTintColor(newTintColor);
        }
        
        public void SetTintToOppositeOrbColor()
        {
            var color = GetColorFromEntityColor(GetOrbColor(), true);
            SetTintColor(color);
        }
        
        public void SetTintToOrbColor()
        {
            var color = GetColorFromEntityColor(GetOrbColor(), false);
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
        
        private static GameEntity.EntityColor GetOrbColor()
        {
            return PlayerGroupController.Instance.orbController.GetColor();
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
    }
}
