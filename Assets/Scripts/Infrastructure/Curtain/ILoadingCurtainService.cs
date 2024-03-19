﻿using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.Curtain
{
    public interface ILoadingCurtainService
    {
        void Show();
        UniTask Hide();
    }
}