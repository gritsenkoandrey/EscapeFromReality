﻿using CodeBase.ECSCore;
using UnityEngine;

namespace CodeBase.Game.Components
{
    public sealed class CCharacterController : EntityComponent<CCharacterController>
    {
        [SerializeField] private CharacterController _characterController;
        
        public CharacterController CharacterController => _characterController;
        public float Angle => transform.eulerAngles.y;
        public float BaseSpeed { get; private set; }
        public float Speed { get; private set; }
        public bool IsGrounded => _characterController.isGrounded;

        public void SetBaseSpeed(float baseSpeed) => BaseSpeed = baseSpeed;
        public void SetSpeed(float speed) => Speed = speed;
    }
}