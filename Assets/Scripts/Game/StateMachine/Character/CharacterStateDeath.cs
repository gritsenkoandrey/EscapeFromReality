﻿using CodeBase.ECSCore;
using CodeBase.Game.Interfaces;
using CodeBase.Infrastructure.CameraMain;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.Models;

namespace CodeBase.Game.StateMachine.Character
{
    public sealed class CharacterStateDeath : CharacterState, IState
    {
        public CharacterStateDeath(IStateMachine stateMachine, ICharacter character, ICameraService cameraService, 
            IJoystickService joystickService, LevelModel levelModel) 
            : base(stateMachine, character, cameraService, joystickService, levelModel)
        {
        }

        void IState.Enter()
        {
            Character.Animator.OnDeath.Execute();
            Character.Entity.CleanSubscribe();
        }

        void IState.Exit() { }

        void IState.Tick() { }
    }
}