using System;
using UnityEngine;

namespace OrbOfDeception
{
    public class ColliderEventTrigger : MonoBehaviour
    {
        [SerializeField] private bool disableWhenTrigger = true;
        [SerializeField] private LayerMask layersToTrigger;
        [HideInInspector] public Action onTrigger;

        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (layersToTrigger == (layersToTrigger | 1 << other.gameObject.layer))
            {
                onTrigger?.Invoke();
                if (disableWhenTrigger)
                    _collider.enabled = false;
            }
        }
    }
}
