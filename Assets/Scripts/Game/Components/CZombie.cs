﻿using CodeBase.ECSCore;
using CodeBase.Game.Enums;
using CodeBase.Game.Interfaces;
using CodeBase.Infrastructure.StaticData.Data;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Game.Components
{
    public sealed class CZombie : EntityComponent<CZombie>, IEnemy
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private CAnimator _animator;
        [SerializeField] private CRadar _radar;
        [SerializeField] private CHealth _health;
        [SerializeField] private CMelee _melee;

        public NavMeshAgent Agent => _agent;
        public CAnimator Animator => _animator;
        public CRadar Radar => _radar;
        public CHealth Health => _health;
        public CMelee Melee => _melee;
        public CCharacter Character { get; private set; }
        public ZombieStats Stats { get; set; }
        public bool IsAggro { get; set; }
        public EnemyState State { get; set; }
        public Vector3 Position => transform.position;
        public void Construct(CCharacter character) => Character = character;

        public ReactiveCommand<int> DamageReceived { get; } = new();
        public ReactiveCommand UpdateStateMachine { get; } = new();
        
        protected override void OnEntityCreate() { }
        protected override void OnEntityEnable() { }
        protected override void OnEntityDisable() { }
    }
}