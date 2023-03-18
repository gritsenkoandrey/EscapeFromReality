﻿using System;
using System.Collections.Generic;
using CodeBase.ECSCore;
using CodeBase.Game.Systems;
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
                new SCharacterAnimationController(),
                new SCharacterShoot(),
                new SSpawnerZombie(),
                new SEnemyStateMachine(),
                new SEnemyAnimationController(),
                new SEnemyCollision(),
                new SEnemyMeleeAttack(),
                new SRadarDraw(),
                new SVirtualCamera(),
                new SSelectMesh(),
                new SUIUpdateHealth(),
                new SInput(),
                new SHealthProvider(),
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