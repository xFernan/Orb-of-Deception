using System;
using System.Collections.Generic;
using System.Linq;
using OrbOfDeception.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OrbOfDeception.Enemy
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
            _enemyController.BaseParameters.onMaskColorChange += UpdateMask;
            
            for (var i = 0; i < whiteMaskSpritesheet.Length; i++)
            {
                _nameIndexDictionary.Add(whiteMaskSpritesheet[i].name, i);
            }
        }

        // Runs after setting the Animator frame sprite.
        private void LateUpdate()
        {
            if (!_nameIndexDictionary.ContainsKey(_spriteRenderer.sprite.name))
            {
                Debug.Log(_spriteRenderer.sprite.name);
                Debug.Log(_nameIndexDictionary.Count);
            }
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
        
        [Button]
        private void UpdateMask()
        {
            var maskColor = GetComponentInParent<EnemyParameters>().MaskColor;

            _spriteRenderer.sprite = maskColor switch
            {
                GameEntity.EntityColor.Black => blackMaskSpritesheet[0],
                GameEntity.EntityColor.White => whiteMaskSpritesheet[0],
                _ => _spriteRenderer.sprite
            };
        }

        private void OnDisable()
        {
            _enemyController.BaseParameters.onMaskColorChange -= UpdateMask;
        }
    }
}
