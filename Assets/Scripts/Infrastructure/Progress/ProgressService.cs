﻿using System;
using CodeBase.Infrastructure.Progress.Data;
using JetBrains.Annotations;

namespace CodeBase.Infrastructure.Progress
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class ProgressService : IProgressService
    {
        public IData<int> LevelData { get; private set; }
        public IData<int> MoneyData { get; private set; }
        public IData<Stats> StatsData { get; private set; }
        public IData<Inventory> InventoryData { get; private set; }
        public IData<Shop> ShopData { get; private set; }
        public IData<bool> HapticData { get; private set; }
        public IData<DailyTask> DailyTaskData { get; private set; }

        void IProgressService.Init()
        {
            LevelData = new LevelData();
            MoneyData = new MoneyData();
            StatsData = new StatsData();
            InventoryData = new InventoryData();
            ShopData = new ShopData();
            HapticData = new HapticData();
            DailyTaskData = new DailyTaskData();
        }

        void IDisposable.Dispose()
        {
            LevelData?.Dispose();
            MoneyData?.Dispose();
            StatsData?.Dispose();
            InventoryData?.Dispose();
            ShopData?.Dispose();
            HapticData?.Dispose();
            DailyTaskData?.Dispose();
        }
    }
}