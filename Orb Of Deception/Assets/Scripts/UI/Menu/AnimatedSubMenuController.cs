using System.Collections;
using System.Collections.Generic;
using OrbOfDeception.Core;
using UnityEngine;
using UnityEngine.Events;

namespace OrbOfDeception.UI
{
    public class AnimatedSubMenuController : SubMenuController
    {
        [System.Serializable]
        private class SequentialEvent
        {
            public float timeToTrigger;
            public UnityEvent eventToTrigger;
        }

        [SerializeField] private SequentialEvent[] sequentialEvents;
        
        private List<Coroutine> _openCoroutines;
        private HideableElement[] _elements;

        protected override void Awake()
        {
            base.Awake();
            
            _elements = GetComponentsInChildren<HideableElement>();
            _openCoroutines = new List<Coroutine>();
        }

        public override void Open()
        {
            base.Open();

            foreach (var sequentialEvent in sequentialEvents)
            {
                var newCoroutine = StartCoroutine(ElementShowingCoroutine(sequentialEvent));
                _openCoroutines.Add(newCoroutine);
            }
        }

        private static IEnumerator ElementShowingCoroutine(SequentialEvent sequentialEvent)
        {
            yield return new WaitForSecondsRealtime(sequentialEvent.timeToTrigger);
            sequentialEvent.eventToTrigger?.Invoke();
        }

        public override void Close()
        {
            base.Close();
            
            StopCoroutines();
            
            foreach (var element in _elements)
            {
                element.Hide();
            }
        }

        private void StopCoroutines()
        {
            foreach (var openCoroutine in _openCoroutines)
            {
                StopCoroutine(openCoroutine);
            }
            
            _openCoroutines.Clear();
        }
    }
}