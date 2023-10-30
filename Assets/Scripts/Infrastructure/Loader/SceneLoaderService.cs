﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Loader
{
    public sealed class SceneLoaderService : ISceneLoaderService
    {
        void ISceneLoaderService.Load(string name, Action onLoaded)
        {
            LoadScene(name, onLoaded).Forget();
        }
        
        private async UniTaskVoid LoadScene(string name, Action onLoaded)
        {
            if (SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                
                return;
            }
            
            await SceneManager.LoadSceneAsync(name).ToUniTask();
            
            onLoaded?.Invoke();
        }
    }
}