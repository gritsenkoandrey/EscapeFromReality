﻿using System;
using System.Collections.Generic;
using CodeBase.ECSCore;
using CodeBase.Game.Systems;
using CodeBase.Game.SystemsUi;
using VContainer.Unity;

namespace CodeBase.LifeTime
{
    public sealed class SystemEntryPoint : IInitializable, IDisposable, ITickable
    {
        private List<SystemBase> _systems;

        public void Initialize()
        {
            CreateSystems();
            EnableSystems();
        }

        public void Dispose()
        {
            DisableSystems();
            Clear();
        }

        public void Tick()
        {
            UpdateSystems();
        }

        private void CreateSystems()
        {
            _systems = new List<SystemBase>
            {
                new SGroundBuildNavMesh(),
                new SCharacterStateMachine(),
                new SCharacterAnimator(),
                new SCharacterWeapon(),
                new SCharacterDeath(),
                new SSpawnerZombie(),
                new SEnemyStateMachine(),
                new SEnemyAnimator(),
                new SEnemyCollision(),
                new SEnemyMeleeAttack(),
                new SEnemyDeath(),
                new SHealthViewUpdate(),
                new SRadarDraw(),
                new SVirtualCamera(),
                new SSelectMesh(),
                new SCharacterInput(),
                
                new SUpgradeShop(),
                new SUpgradeButton(),
            };
        }

        private void EnableSystems()
        {
            foreach (SystemBase system in _systems)
            {
                system.EnableSystem();
            }
        }

        private void DisableSystems()
        {
            foreach (SystemBase system in _systems)
            {
                system.DisableSystem();
            }
        }

        private void UpdateSystems()
        {
            foreach (SystemBase system in _systems)
            {
                system.Tick();
            }
        }

        private void Clear()
        {
            _systems.Clear();
        }
    }
}