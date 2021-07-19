using System;
using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Orb;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception
{
    public class MedallionOrbController : MonoBehaviour
    {
        [SerializeField] private Sprite whiteOrb;
        [SerializeField] private Sprite blackOrb;

        private Image _orbImage;

        private void Awake()
        {
            _orbImage = GetComponent<Image>();

            OrbController.onChangeOrbColor += OnChangeOrbColor;
        }

        private void OnChangeOrbColor(GameEntity.EntityColor newOrbColor)
        {
            _orbImage.sprite = newOrbColor switch
            {
                GameEntity.EntityColor.White => whiteOrb,
                GameEntity.EntityColor.Black => blackOrb,
                GameEntity.EntityColor.Other => whiteOrb,
                _ => throw new ArgumentOutOfRangeException(nameof(newOrbColor), newOrbColor, null)
            };
        }
        
    }
}
