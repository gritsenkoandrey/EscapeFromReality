﻿using System.Collections.Generic;
using CodeBase.Game.ComponentsUi;
using CodeBase.Game.Enums;
using CodeBase.Infrastructure.Progress;
using CodeBase.Infrastructure.Services;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.UI
{
    public interface IUIFactory : IService
    {
        public List<IProgressReader> ProgressReaders { get; }
        public List<IProgressWriter> ProgressWriters { get; }
        public StaticCanvas CreateCanvas();
        public BaseScreen CreateScreen(ScreenType type);
        public CUpgradeButton CreateUpgradeButton(UpgradeButtonType type, Transform parent);
        public void CleanUp();
    }
}