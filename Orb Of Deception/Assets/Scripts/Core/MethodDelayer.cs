using System;
using System.Collections;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public class MethodDelayer
    {
        private readonly Action _methodCallback;
        private readonly MonoBehaviour _monoBehaviour;

        private Coroutine _delayCoroutine;

        public MethodDelayer(MonoBehaviour monoBehaviour, Action methodCallback)
        {
            _monoBehaviour = monoBehaviour;
            _methodCallback = methodCallback;
        }

        private IEnumerator MethodCoroutine(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _methodCallback?.Invoke();
        }
        
        public void SetNewDelay(float methodDelay)
        {
            _delayCoroutine = _monoBehaviour.StartCoroutine(MethodCoroutine(methodDelay));
        }

        public void StopDelay()
        {
            if (_delayCoroutine == null) return;
            
            _monoBehaviour.StopCoroutine(_delayCoroutine);
        }
    }
}