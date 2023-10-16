﻿using CodeBase.ECSCore;
using CodeBase.Game.Components;
using CodeBase.Game.Interfaces;
using CodeBase.Game.StateMachine.Zombie;
using CodeBase.Infrastructure.Factories.Game;
using CodeBase.Infrastructure.States;
using UniRx;

namespace CodeBase.Game.Systems
{
    public sealed class SLevelGameState : SystemComponent<CCharacter>
    {
        private readonly IGameStateService _gameStateService;
        private readonly IGameFactory _gameFactory;

        public SLevelGameState(IGameStateService gameStateService, IGameFactory gameFactory)
        {
            _gameStateService = gameStateService;
            _gameFactory = gameFactory;
        }

        protected override void OnEnableSystem()
        {
            base.OnEnableSystem();
        }

        protected override void OnDisableSystem()
        {
            base.OnDisableSystem();
        }

        protected override void OnEnableComponent(CCharacter component)
        {
            base.OnEnableComponent(component);
            
            SubscribeOnWinGame(component);
            SubscribeOnFailGame(component);
        }

        protected override void OnDisableComponent(CCharacter component)
        {
            base.OnDisableComponent(component);
        }

        private void SubscribeOnWinGame(CCharacter component)
        {
            _gameFactory.Enemies
                .ObserveRemove()
                .Subscribe(_ =>
                {
                    if (_gameFactory.Enemies.Count == 0)
                    {
                        _gameStateService.Enter<StateWin>();
                        
                        component.LifetimeDisposable.Clear();
                    }
                })
                .AddTo(component.LifetimeDisposable);
        }

        private void SubscribeOnFailGame(CCharacter component)
        {
            component.Health.Health
                .SkipLatestValueOnSubscribe()
                .ObserveOnMainThread()
                .Subscribe(_ =>
                {
                    if (!_gameFactory.Character.Health.IsAlive)
                    {
                        _gameStateService.Enter<StateFail>();
                        
                        component.LifetimeDisposable.Clear();

                        foreach (IEnemy enemy in _gameFactory.Enemies)
                        {
                            enemy.StateMachine.Enter<ZombieStateNone>();
                        }
                    }
                })
                .AddTo(component.LifetimeDisposable);
        }
    }
}