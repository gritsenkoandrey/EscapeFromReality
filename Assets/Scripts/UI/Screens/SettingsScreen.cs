﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;

namespace CodeBase.UI.Screens
{
    public sealed class SettingsScreen : BaseScreen
    {
        private Tween _tween;

        private protected override void OnEnable()
        {
            base.OnEnable();

            _button
                .OnClickAsObservable()
                .First()
                .Subscribe(_ => Hide().Forget())
                .AddTo(LifeTimeDisposable);
            
            Show().Forget();
        }

        private protected override async UniTask Show()
        {
            await base.Show();
            
            _tween = BounceButton();
        }

        private protected override async UniTask Hide()
        {
            _tween?.Kill();
            
            await base.Hide();
            
            await FadeCanvas(1f, 0f).AsyncWaitForCompletion().AsUniTask();
            
            CloseScreen.Execute();
        }
    }
}