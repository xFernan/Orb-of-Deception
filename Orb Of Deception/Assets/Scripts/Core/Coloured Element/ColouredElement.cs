using OrbOfDeception.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public abstract class ColouredElement : MonoBehaviour
    {
        [SerializeField] private bool isOppositeMaskColor;
        [SerializeField] protected Color blackColor;
        [SerializeField] protected Color whiteColor;
        
        [SerializeField] private bool haveOtherColor;
        [ShowIf("haveOtherColor", true)]
        [SerializeField] protected Color otherColor;

        protected virtual void Awake()
        {
            var maskColor = GetComponentInParent<EnemyParameters>().MaskColor;

            switch (maskColor)
            {
                case GameEntity.EntityColor.Black:
                    if (isOppositeMaskColor)
                        SetToWhite();
                    else
                        SetToBlack();
                    break;
                case GameEntity.EntityColor.White:
                    if (isOppositeMaskColor)
                        SetToBlack();
                    else
                        SetToWhite();
                    break;
                case GameEntity.EntityColor.Other:
                    SetToOtherColor();
                    break;
            }
        }

        protected abstract void SetToBlack();
        protected abstract void SetToWhite();
        protected abstract void SetToOtherColor();
    }
}