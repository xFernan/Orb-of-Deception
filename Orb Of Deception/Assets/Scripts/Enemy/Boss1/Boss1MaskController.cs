using System;
using System.Collections;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class Boss1MaskController : MonoBehaviour
    {
        private Animator _animator;
        private Boss1Controller _bossController;
        
        private Coroutine _maskColorChangeCoroutine;
        
        private static readonly int ChangeMaskColor = Animator.StringToHash("ChangeMaskColor");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _bossController = GetComponentInParent<Boss1Controller>();
        }

        private IEnumerator ChangeMaskColorCoroutine()
        {
            yield return new WaitForSeconds(_bossController.Parameters.Stats.timeBetweenMaskColorChange);
            
            _animator.SetTrigger(ChangeMaskColor);
            
            _maskColorChangeCoroutine = StartCoroutine(ChangeMaskColorCoroutine());
        }

        private void ChangeMaskColorToOpposite()
        {
            var maskColor = _bossController.BaseParameters.MaskColor;
            maskColor = maskColor == GameEntity.EntityColor.Black ?
                GameEntity.EntityColor.White :
                GameEntity.EntityColor.Black;
            _bossController.BaseParameters.MaskColor = maskColor;
            
            _bossController.soundsPlayer.Play("ChangingMaskColor");
        }
        
        public void StartMaskColorChange()
        {
            _maskColorChangeCoroutine = StartCoroutine(ChangeMaskColorCoroutine());
        }
        
        public void StopChangingColor()
        {
            StopCoroutine(_maskColorChangeCoroutine);
        }
    }
}
