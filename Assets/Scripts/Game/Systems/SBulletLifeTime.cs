﻿using System;
using CodeBase.ECSCore;
using CodeBase.Game.Components;
using CodeBase.Game.Interfaces;
using CodeBase.Infrastructure.Pool;
using UniRx;

namespace CodeBase.Game.Systems
{
    public sealed class SBulletLifeTime : SystemComponent<CBullet>
    {
        private readonly IObjectPoolService _objectPoolService;

        public SBulletLifeTime(IObjectPoolService objectPoolService)
        {
            _objectPoolService = objectPoolService;
        }

        protected override void OnEnableComponent(CBullet component)
        {
            base.OnEnableComponent(component);

            component.OnDestroy
                .First()
                .Subscribe(_ => ReturnToPool(component))
                .AddTo(component.LifetimeDisposable);

            Observable.Timer(Time())
                .First()
                .Subscribe(_ => ReturnToPool(component))
                .AddTo(component.LifetimeDisposable);
        }

        private void ReturnToPool(IObject component) => _objectPoolService.ReleaseObject(component.Object);
        
        private TimeSpan Time() => TimeSpan.FromSeconds(2f);
    }
}