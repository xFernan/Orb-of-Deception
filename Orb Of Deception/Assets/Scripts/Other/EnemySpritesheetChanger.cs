using System;
using System.Collections.Generic;
using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using UnityEngine;

namespace OrbOfDeception.Other
{
    public class EnemySpritesheetChanger : MonoBehaviour
    {
        [SerializeField] private Sprite[] whiteMaskSpritesheet;
        [SerializeField] private Sprite[] blackMaskSpritesheet;

        private Dictionary<string, int> _nameIndexDictionary;

        private SpriteRenderer _spriteRenderer;
        private EnemyController _enemyController;

        private void Awake()
        {
            _nameIndexDictionary = new Dictionary<string, int>();
            
            _enemyController = GetComponentInParent<EnemyController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            for (var i = 0; i < whiteMaskSpritesheet.Length; i++)
            {
                _nameIndexDictionary.Add(whiteMaskSpritesheet[i].name, i);
            }
        }

        // Runs after setting the Animator frame sprite.
        private void LateUpdate()
        {
            var frameIndex = _nameIndexDictionary[_spriteRenderer.sprite.name];
            _spriteRenderer.sprite = GetCurrentSpriteSheet()[frameIndex];
        }

        private Sprite[] GetCurrentSpriteSheet()
        {
            return _enemyController.GetMaskColor() switch
            {
                GameEntity.EntityColor.White => whiteMaskSpritesheet,
                GameEntity.EntityColor.Black => blackMaskSpritesheet,
                GameEntity.EntityColor.Other => whiteMaskSpritesheet,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
