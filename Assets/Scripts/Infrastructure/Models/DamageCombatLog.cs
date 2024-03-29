﻿using System;
using System.Collections.Generic;
using CodeBase.Game.Interfaces;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace CodeBase.Infrastructure.Models
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class DamageCombatLog : IDisposable
    {
        public readonly ReactiveCommand<CombatLog> CombatLog = new ();

        private readonly Queue<CombatLog> _damageCombatLog;
        private float _time;

        private const float UpdateLogTime = 0.1f;

        public DamageCombatLog()
        {
            _damageCombatLog = new Queue<CombatLog>();
        }

        public void AddLog(ITarget target, int damage)
        {
            _damageCombatLog.Enqueue(new CombatLog(target, damage));
        }

        public void Execute()
        {
            if (_damageCombatLog.Count == 0)
            {
                return;
            }
            
            _time += Time.deltaTime;

            if (_time > UpdateLogTime)
            {
                _time = 0f;

                if (_damageCombatLog.TryDequeue(out CombatLog log))
                {
                    CombatLog.Execute(log);
                }
            }
        }

        void IDisposable.Dispose()
        {
            _damageCombatLog.Clear();
            CombatLog?.Dispose();
        }
    }

    public readonly struct CombatLog
    {
        public readonly ITarget Target;
        public readonly int Damage;

        public CombatLog(ITarget target, int damage)
        {
            Target = target;
            Damage = damage;
        }
    }
}