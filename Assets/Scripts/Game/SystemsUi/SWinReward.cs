﻿using CodeBase.ECSCore;
using CodeBase.Game.ComponentsUi;
using CodeBase.Infrastructure.Models;
using CodeBase.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace CodeBase.Game.SystemsUi
{
    public sealed class SWinReward : SystemComponent<CWinReward>
    {
        private LevelModel _levelModel;
        private LootModel _lootModel;

        [Inject]
        private void Construct(LevelModel levelModel, LootModel lootModel)
        {
            _levelModel = levelModel;
            _lootModel = lootModel;
        }

        protected override void OnEnableComponent(CWinReward component)
        {
            base.OnEnableComponent(component);

            ActivateStar(component);
            CalculateLoot(component);
            ShowAnimation(component);
        }

        private void ActivateStar(CWinReward component)
        {
            int index = 0;

            if (CharacterHaseFullHealth())
            {
                index++;
            }

            if (CharacterHasHalfHealth())
            {
                index++;
            }

            if (LevelCompletedHalfTime())
            {
                index++;
            }

            for (int i = 0; i < component.Stars.Length; i++)
            {
                component.Stars[i].gameObject.SetActive(index > i);
            }
        }

        private bool CharacterHaseFullHealth() => 
            _levelModel.Character.Health.CurrentHealth.Value == _levelModel.Character.Health.MaxHealth;
        
        private bool CharacterHasHalfHealth() => 
            _levelModel.Character.Health.CurrentHealth.Value >= _levelModel.Character.Health.MaxHealth / 2;

        private bool LevelCompletedHalfTime() => 
            _levelModel.Level.Time >= _levelModel.Level.MaxTime / 2;

        private void CalculateLoot(CWinReward component)
        {
            component.Text.text = _lootModel.GenerateLevelLoot(_levelModel.Level).Trim();
        }

        private void ShowAnimation(CWinReward component)
        {
            int i = 1;

            foreach (Image star in component.Stars)
            {
                DOTween.Sequence()
                    .AppendInterval(i * 0.2f)
                    .Append(star.transform
                        .DOScale(Vector3.one, 0.5f)
                        .From(Vector3.one * 1.65f)
                        .SetEase(Ease.InCubic))
                    .Join(star
                        .DOFade(1f, 0.4f)
                        .From(0f)
                        .SetEase(Ease.Linear))
                    .Append(star.transform
                        .DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 0.5f))
                    .SetLink(star.gameObject);

                i++;
            }
        }
    }
}