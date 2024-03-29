﻿using UnityEngine;

namespace CodeBase.App
{
    public sealed class AppSettings
    {
        public void SetSettings()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
            Debug.unityLogger.logEnabled = Debug.isDebugBuild;
        }
    }
}