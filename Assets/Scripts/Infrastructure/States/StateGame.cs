﻿using System;
using CodeBase.Game.Interfaces;
using CodeBase.Game.StateMachine.Character;
using CodeBase.Game.StateMachine.Unit;
using CodeBase.Infrastructure.Factories.UI;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.Models;
using CodeBase.UI.Screens;
using CodeBase.Utils;
using UniRx;
using VContainer;

namespace CodeBase.Infrastructure.States
{
    public sealed class StateGame : IEnterState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private IJoystickService _joystickService;
        private IUIFactory _uiFactory;
        private LevelModel _levelModel;

        private readonly CompositeDisposable _transitionDisposable = new();

        public StateGame(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        [Inject]
        private void Construct(IJoystickService joystickService, IUIFactory uiFactory, LevelModel levelModel)
        {
            _joystickService = joystickService;
            _uiFactory = uiFactory;
            _levelModel = levelModel;
        }
        
        void IEnterState.Enter()
        {
            _uiFactory.CreateScreen(ScreenType.Game);
            _joystickService.Enable(true);
            
            ActivateUnitStateMachine();
            
            SubscribeOnWin();
            SubscribeOnLose();
        }

        void IExitState.Exit()
        {
            _joystickService.Enable(false);
            _transitionDisposable.Clear();
        }

        private void ActivateUnitStateMachine()
        {
            _levelModel.Character.StateMachine.StateMachine.Enter<CharacterStateIdle>();
            _levelModel.Enemies.Foreach(SetEnemyStateIdle);
        }

        private void SubscribeOnWin()
        {
            _levelModel.Enemies
                .ObserveRemove()
                .First(_ => AllEnemyIsDeath() && CharacterIsDeath() == false)
                .Subscribe(_ => Win())
                .AddTo(_transitionDisposable);
        }

        private void SubscribeOnLose()
        {
            _levelModel.Character.Health.CurrentHealth
                .First(_ => CharacterIsDeath())
                .Subscribe(_ => Lose())
                .AddTo(_transitionDisposable);
            
            Observable.Timer(TimeSpan.FromSeconds(1f))
                .Repeat()
                .Subscribe(_ => Lose())
                .AddTo(_transitionDisposable);
        }

        private void Win()
        {
            _gameStateMachine.Enter<StateWin>();
            _levelModel.Character.StateMachine.StateMachine.Enter<CharacterStateNone>();
        }

        private void Lose()
        {
            if (_levelModel.Level.Time.Value > 0)
            {
                _levelModel.Level.SpendTime();
                
                return;
            }
            
            _gameStateMachine.Enter<StateFail>();
            _levelModel.Character.StateMachine.StateMachine.Enter<CharacterStateDeath>();
            _levelModel.Enemies.Foreach(SetEnemyStateNone);
        }

        private bool AllEnemyIsDeath() => _levelModel.Enemies.Count == 0;
        private bool CharacterIsDeath() => _levelModel.Character.Health.IsAlive == false;
        private void SetEnemyStateNone(IEnemy enemy) => enemy.StateMachine.StateMachine.Enter<UnitStateNone>();
        private void SetEnemyStateIdle(IEnemy enemy) => enemy.StateMachine.StateMachine.Enter<UnitStateIdle>();
    }
}