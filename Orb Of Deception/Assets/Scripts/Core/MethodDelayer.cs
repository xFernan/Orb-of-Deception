using System;
using System.Collections;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public class MethodDelayer
    {
        private readonly Action _methodCallback;
        private readonly MonoBehaviour _monoBehaviour;
        private bool _alreadyHasADelay = false;

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
            _alreadyHasADelay = true;
        }

        public void StopDelay()
        {
            if (_delayCoroutine == null || !_alreadyHasADelay) return;
            
            _monoBehaviour.StopCoroutine(_delayCoroutine);
            _alreadyHasADelay = false;
        }

        public bool AlreadyHasADelay()
        {
            return _alreadyHasADelay;
        }
    }
}