﻿using CodeBase.ECSCore;
using CodeBase.Utils;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Components
{
    public sealed class CDestroyMeshEffect : EntityComponent<CDestroyMeshEffect>
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public void Init(SkinnedMeshRenderer mesh)
        {
            ParticleSystem.ShapeModule shapeModule = _particleSystem.shape;
            
            shapeModule.skinnedMeshRenderer = mesh;
            
            DOVirtual.Float(1f, 2f, 1f, value => mesh.material.SetFloat(Shaders.Fade, value))
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
            
            _particleSystem.Play();
        }

        public IReactiveProperty<SkinnedMeshRenderer> OnInit { get; } = 
            new ReactiveProperty<SkinnedMeshRenderer>(default);
    }
}