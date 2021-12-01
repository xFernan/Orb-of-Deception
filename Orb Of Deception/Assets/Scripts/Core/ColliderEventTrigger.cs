using System;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public class ColliderEventTrigger : MonoBehaviour
    {
        [SerializeField] private bool disableWhenTrigger = true;
        [SerializeField] private LayerMask layersToTrigger;
        public Action onTrigger;

        private Collider2D[] _colliders;

        private void Awake()
        {
            _colliders = GetComponents<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (layersToTrigger == (layersToTrigger | 1 << other.gameObject.layer))
            {
                if (disableWhenTrigger)
                {
                    foreach (var collider in _colliders)
                    {
                        collider.enabled = false;
                    }
                }
                onTrigger?.Invoke();
            }
        }
    }
}
