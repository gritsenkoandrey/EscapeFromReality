﻿using System.Collections.Generic;
using CodeBase.Game.Components;
using CodeBase.Game.Enums;
using CodeBase.Infrastructure.Progress;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Infrastructure.StaticData.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Game
{
    public sealed class GameFactory : IGameFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressService _progressService;
        
        public List<IProgressReader> ProgressReaders { get; } = new();
        public List<IProgressWriter> ProgressWriters { get; } = new();
        public CLevel CurrentLevel { get; private set; }
        public CCharacter CurrentCharacter { get; private set; }

        public GameFactory(IStaticDataService staticDataService, IProgressService progressService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
        }
        
        CLevel IGameFactory.CreateLevel()
        {
            LevelType levelType = _progressService.PlayerProgress.Level % 5 == 0 ? LevelType.Boss : LevelType.Normal;
            
            return CurrentLevel = Object.Instantiate(_staticDataService.LevelData(levelType).Prefab);
        }

        CCharacter IGameFactory.CreateCharacter()
        {
            CharacterData characterData = _staticDataService.CharacterData();
            
            LevelType levelType = _progressService.PlayerProgress.Level % 5 == 0 ? LevelType.Boss : LevelType.Normal;

            CurrentCharacter = Object.Instantiate(characterData.Prefab, _staticDataService.LevelData(levelType).Prefab.CharacterSpawnPosition, Quaternion.identity);

            CurrentCharacter.Health.BaseHealth = characterData.Health;
            CurrentCharacter.Weapon.BaseDamage = characterData.Damage;
            CurrentCharacter.Weapon.AttackDistance = characterData.AttackDistance;
            CurrentCharacter.Weapon.AttackRecharge = characterData.AttackRecharge;
            CurrentCharacter.Move.BaseSpeed = characterData.Speed;
                
            Registered(CurrentCharacter.Health);
            Registered(CurrentCharacter.Weapon);
            Registered(CurrentCharacter.Move);

            return CurrentCharacter;
        }

        CZombie IGameFactory.CreateZombie(ZombieType zombieType, Vector3 position, Transform parent)
        {
            ZombieData data = _staticDataService.ZombieData(zombieType);
            
            CZombie zombie = Object.Instantiate(data.Prefab, position, Quaternion.identity, parent);
            
            zombie.Construct(CurrentCharacter);

            zombie.Health.MaxHealth = data.Health;
            zombie.Health.Health.Value = data.Health;
            zombie.Melee.Damage = data.Damage;
            zombie.Stats = data.Stats;
            
            CurrentCharacter.Enemies.Add(zombie);

            return zombie;
        }

        CBullet IGameFactory.CreateBullet(Vector3 position)
        {
            return Object.Instantiate(_staticDataService.BulletData(), position, Quaternion.identity);
        }

        Transform IGameFactory.CreateHitFx(Vector3 position)
        {
            return Object.Instantiate(_staticDataService.FxData().HitFx, position, Quaternion.identity);
        }

        Transform IGameFactory.CreateDeathFx(Vector3 position)
        {
            return Object.Instantiate(_staticDataService.FxData().DeatFx, position, Quaternion.identity);
        }

        void IGameFactory.CleanUp()
        {
            if (CurrentCharacter != null)
            {
                Object.Destroy(CurrentCharacter.gameObject);
            }
            
            if (CurrentLevel != null)
            {
                Object.Destroy(CurrentLevel.gameObject);
            }

            CurrentLevel = null;
            CurrentCharacter = null;
            
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private void Registered(IProgress progress)
        {
            if (progress is IProgressWriter writer)
            {
                ProgressWriters.Add(writer);
            }

            if (progress is IProgressReader reader)
            {
                ProgressReaders.Add(reader);
            }
        }
    }
}