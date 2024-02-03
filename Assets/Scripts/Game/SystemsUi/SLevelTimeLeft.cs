﻿using System;
using CodeBase.ECSCore;
using CodeBase.Game.ComponentsUi;
using CodeBase.Infrastructure.Models;
using CodeBase.Utils;
using UniRx;
using VContainer;

namespace CodeBase.Game.SystemsUi
{
    public sealed class SLevelTimeLeft : SystemComponent<CLevelTimeLeft>
    {
        private LevelModel _levelModel;

        [Inject]
        private void Construct(LevelModel levelModel)
        {
            _levelModel = levelModel;
        }

        protected override void OnEnableComponent(CLevelTimeLeft component)
        {
            base.OnEnableComponent(component);

            SubscribeOnUpdateTimeLeft(component);
        }

        private void SubscribeOnUpdateTimeLeft(CLevelTimeLeft component)
        {
            int time = _levelModel.Level.Time;

            Observable.Timer(Time())
                .Repeat()
                .Where(_ => time > 0)
                .DoOnSubscribe(() => SetTimeLeftText(component, time))
                .Subscribe(_ => UpdateTime(component, ref time))
                .AddTo(component.LifetimeDisposable);
        }

        private TimeSpan Time() => TimeSpan.FromSeconds(1f);

        private void UpdateTime(CLevelTimeLeft component, ref int time)
        {
            time -= 1;
            SetTimeLeftText(component, time);
        }

        private void SetTimeLeftText(CLevelTimeLeft component, int time)
        {
            component.TimeLeftText.text = SpriteAssetExtension.Timer + FormatTime.SecondsToTime(time);
        }
    }
}