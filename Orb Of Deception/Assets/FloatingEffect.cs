using System;
using UnityEngine;

namespace OrbOfDeception
{
    public class FloatingEffect : MonoBehaviour
    {
        [SerializeField] private float floatingMoveDistance = 0.1f;
        [SerializeField] private float floatingMoveVelocity = 5;
        private Vector3 initialPosition;
        
        private void Awake()
        {
            initialPosition = transform.localPosition;
        }

        private void Update()
        {
            transform.localPosition = initialPosition + Mathf.Sin(Time.time * floatingMoveVelocity) * floatingMoveDistance * Vector3.up;
        }
    }
}
