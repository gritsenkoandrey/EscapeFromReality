﻿using CodeBase.ECSCore;
using CodeBase.Game.ComponentsUi;
using CodeBase.Infrastructure.Models;
using UniRx;
using VContainer;

namespace CodeBase.Game.SystemsUi
{
    public sealed class SCharacterAmmunitionView : SystemComponent<CCharacterAmmunitionView>
    {
        private InventoryModel _inventoryModel;

        [Inject]
        public void Construct(InventoryModel inventoryModel)
        {
            _inventoryModel = inventoryModel;
        }
        
        protected override void OnEnableComponent(CCharacterAmmunitionView component)
        {
            base.OnEnableComponent(component);

            _inventoryModel.ClipCount
                .Subscribe(count =>
                {
                    component.AmmunitionCount.text = count > 0 ? count.ToString() : "R";
                })
                .AddTo(component.LifetimeDisposable);
        }
    }
}