﻿using CodeBase.Game.Builders;
using CodeBase.Game.Components;
using CodeBase.Game.Interfaces;
using CodeBase.Infrastructure.AssetData;
using CodeBase.Infrastructure.Models;
using CodeBase.Infrastructure.Pool;
using CodeBase.Infrastructure.Progress;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Infrastructure.StaticData.Data;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace CodeBase.Game.Weapon.Factories
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class WeaponFactory : IWeaponFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetService _assetService;
        private readonly IObjectPoolService _objectPoolService;
        private readonly IProgressService _progressService;
        private readonly InventoryModel _inventoryModel;

        public WeaponFactory(
            IStaticDataService staticDataService, 
            IAssetService assetService, 
            IObjectPoolService objectPoolService,
            IProgressService progressService,
            InventoryModel inventoryModel)
        {
            _staticDataService = staticDataService;
            _assetService = assetService;
            _objectPoolService = objectPoolService;
            _progressService = progressService;
            _inventoryModel = inventoryModel;
        }

        async UniTask<CWeapon> IWeaponFactory.CreateCharacterWeapon(WeaponType type, Transform parent)
        {
            WeaponCharacteristicData data = _staticDataService.WeaponCharacteristicData(type);

            GameObject prefab = await _assetService.LoadFromAddressable<GameObject>(data.PrefabReference);

            return new WeaponCharacterBuilder(this, data.WeaponCharacteristic, _progressService, _inventoryModel)
                .SetPrefab(prefab)
                .SetParent(parent)
                .Build();
        }

        async UniTask<CWeapon> IWeaponFactory.CreateUnitWeapon(WeaponType type, Transform parent)
        {
            WeaponCharacteristicData data = _staticDataService.WeaponCharacteristicData(type);
            
            GameObject prefab = await _assetService.LoadFromAddressable<GameObject>(data.PrefabReference);

            return new WeaponUnitBuilder(this, data.WeaponCharacteristic)
                .SetPrefab(prefab)
                .SetParent(parent)
                .Build();
        }

        async UniTask<IBullet> IWeaponFactory.CreateProjectile(ProjectileType type, Transform spawnPoint, int damage, Vector3 direction)
        {
            ProjectileData data = _staticDataService.ProjectileData(type);
            
            GameObject prefab = await _assetService.LoadFromAddressable<GameObject>(data.PrefabReference);

            return new ProjectileBuilder(_objectPoolService)
                .SetPrefab(prefab)
                .SetSpawnPoint(spawnPoint)
                .SetDamage(damage)
                .SetDirection(direction)
                .SetCollisionDistance(data.CollisionRadius)
                .Build();
        }
    }
}