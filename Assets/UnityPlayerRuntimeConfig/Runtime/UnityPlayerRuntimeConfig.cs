using System;
using UnityEngine;

namespace work.ctrl3d
{
    public static class UnityPlayerRuntimeConfig
    {
        public static void Apply()
        {
            RuntimeConfigManager.Load();

            var appConfig = RuntimeConfigManager.GetApplicationConfig();
            var screenConfig = RuntimeConfigManager.GetScreenConfig();
            var qualityConfig = RuntimeConfigManager.GetQualityConfig();
            var audioConfig = RuntimeConfigManager.GetAudioConfig();
            var cameraConfig = RuntimeConfigManager.GetCameraConfig();
            var windowConfig = RuntimeConfigManager.GetWindowConfig();
            
            // Application
            Application.runInBackground = appConfig.runInBackground;
            Application.targetFrameRate = appConfig.targetFrameRate;

            // Screen
            Screen.fullScreen = screenConfig.fullScreen;

            // Quality
            QualitySettings.vSyncCount = qualityConfig.vSyncCount;
            QualitySettings.SetQualityLevel(qualityConfig.qualityLevel);

            // Audio
            AudioListener.volume = audioConfig.volume;

            // Camera
            if (Camera.main != null)
            {
                if (ColorUtility.TryParseHtmlString(cameraConfig.backgroundColor, out var backgroundColor))
                {
                    Camera.main.backgroundColor = backgroundColor;
                }

                Camera.main.fieldOfView = cameraConfig.fieldOfView;
                Camera.main.nearClipPlane = cameraConfig.nearClipPlane;
                Camera.main.farClipPlane = cameraConfig.farClipPlane;
            }

#if !UNITY_EDITOR
    #if UNITY_STANDALONE_WIN

            // (Windows 전용)
            var hWnd = WinUtil.GetUnityHandle();

            if (hWnd != IntPtr.Zero)
            {
                WinUtil.SetFocus(hWnd);

                if (windowConfig.popupWindow)
                {
                    WinUtil.SetPopupWindow(hWnd);
                }
                else
                {
                    WinUtil.CancelPopupWindow(hWnd);
                }

                if (screenConfig.width > 0 && screenConfig.height > 0)
                {
                    WinUtil.MoveAndResizeWindow(hWnd,
                        screenConfig.x, screenConfig.y,
                        screenConfig.width, screenConfig.height);
                }

                if (!string.IsNullOrEmpty(windowConfig.zOrder))
                {
                    var zOrder = windowConfig.zOrder.ToUpper();
                    switch (zOrder)
                    {
                        case "BOTTOM":
                            WinUtil.SetFocus(hWnd);
                            WinUtil.Bottom(hWnd);
                            break;
                        case "NOTOPMOST":
                            WinUtil.SetFocus(hWnd);
                            WinUtil.NoTopMost(hWnd);
                            break;
                        case "TOP":
                            WinUtil.SetFocus(hWnd);
                            WinUtil.Top(hWnd);
                            break;
                        case "TOPMOST":
                            WinUtil.SetFocus(hWnd);
                            WinUtil.TopMost(hWnd);
                            break;
                    }
                }
            }
    #endif
            
    #if UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
            if (screenConfig.width > 0 && screenConfig.height > 0)
            {
                Screen.SetResolution(screenConfig.width, screenConfig.height, screenConfig.fullScreenMode);
            }
    #endif
#endif
        }
    }
}