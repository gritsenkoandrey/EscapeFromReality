﻿using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetData
{
    public interface IAssetService : IService
    {
        public T Load<T>(string path) where T : Object;
        public T[] LoadAll<T>(string path) where T : Object;
        public void Unload();
    }
}