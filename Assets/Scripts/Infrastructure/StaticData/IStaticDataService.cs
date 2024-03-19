﻿using CodeBase.Game.Enums;
using CodeBase.Infrastructure.StaticData.Data;
using CodeBase.UI.Screens;

namespace CodeBase.Infrastructure.StaticData
{
    public interface IStaticDataService
    {
        void Load();
        ScreenData ScreenData(ScreenType type);
        UpgradeButtonData UpgradeButtonData(UpgradeButtonType type);
        WeaponCharacteristicData WeaponCharacteristicData(WeaponType type);
        ProjectileData ProjectileData(ProjectileType type);
        EffectData EffectData(EffectType type);
        LevelData LevelData();
        CharacterData CharacterData();
        TextureData TextureArrayData();
        UiData UiData();
        PoolData PoolData();
        UnitData UnitData();
        PreviewData PreviewData();
        ShopData ShopData();
    }
}