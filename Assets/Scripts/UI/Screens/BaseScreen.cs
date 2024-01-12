﻿using CodeBase.Infrastructure.States;
using CodeBase.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace CodeBase.UI.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _safeArea;

        private protected IGameStateService GameStateService { get; private set; }

        [Inject]
        public void Construct(IGameStateService gameStateService)
        {
            GameStateService = gameStateService;
        }

        protected virtual void OnEnable() => _safeArea.ApplySafeArea();

        protected virtual void OnDisable() { }
        
        private protected Tween FadeCanvas(float from, float to, float duration)
        {
            return _canvasGroup
                .DOFade(to, duration)
                .From(from)
                .SetEase(Ease.Linear);
        }

        private protected Tween BounceButton(Button button, float to, float duration)
        {
            return button.transform
                .DOScale(Vector3.one * to, duration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}