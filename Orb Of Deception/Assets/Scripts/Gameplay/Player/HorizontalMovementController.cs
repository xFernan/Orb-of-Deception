﻿using OrbOfDeception.Core.Input;
using UnityEngine;

namespace OrbOfDeception.Gameplay.Player
{
    public class HorizontalMovementController
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly float _velocity;
        private readonly InputManager _inputManager;

        public HorizontalMovementController(Rigidbody2D rigidbody, float velocity, InputManager inputManager)
        {
            _rigidbody = rigidbody;
            _velocity = velocity;
            _inputManager = inputManager;
        }

        public void FixedUpdate()
        {
            var newVelocity = _rigidbody.velocity;
            newVelocity.x = _velocity * _inputManager.GetHorizontal();
            _rigidbody.velocity = newVelocity;
        }
    }
}