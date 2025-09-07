#if USE_JSONCONFIG
using System.IO;
using UnityEngine;

namespace work.ctrl3d
{
    public static class UnityPlayerRuntimeConfig
    {
        public static void Apply()
        {
            var runtimeConfigPath = Path.Combine(Application.dataPath, "UnityPlayerRuntimeConfig.json");
            var runtimeConfig = new JsonConfig<RuntimeConfig>(runtimeConfigPath).Load().Data;

            var appConfig = runtimeConfig.application;
            var screenConfig = runtimeConfig.screen;
            var qualityConfig = runtimeConfig.quality;
            var audioConfig = runtimeConfig.audio;
            var cameraConfig = runtimeConfig.camera;
            var windowConfig = runtimeConfig.window;
            var cursorConfig = runtimeConfig.cursor;

            // Application
            Application.runInBackground = appConfig.runInBackground;
            Application.targetFrameRate = appConfig.targetFrameRate;
            Application.backgroundLoadingPriority = (ThreadPriority)appConfig.backgroundLoadingPriority;

            // Screen
            Screen.brightness = screenConfig.brightness;
            Screen.fullScreen = screenConfig.fullScreen;
            Screen.orientation = (ScreenOrientation)screenConfig.orientation;
            Screen.sleepTimeout = screenConfig.sleepTimeout;

            // Quality
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)qualityConfig.anisotropicFiltering;
            QualitySettings.vSyncCount = qualityConfig.vSyncCount;
            QualitySettings.SetQualityLevel(qualityConfig.qualityLevel);
            QualitySettings.antiAliasing = qualityConfig.antiAliasing;

            // Audio
            AudioListener.volume = audioConfig.volume;

            // Camera
            if (Camera.main != null)
            {
                Camera.main.clearFlags = (CameraClearFlags)cameraConfig.clearFlags;

                if (ColorUtility.TryParseHtmlString(cameraConfig.backgroundColor, out var backgroundColor))
                {
                    Camera.main.backgroundColor = backgroundColor;
                }

                Camera.main.orthographic = cameraConfig.orthographic;
                Camera.main.orthographicSize = cameraConfig.orthographicSize;
                Camera.main.fieldOfView = cameraConfig.fieldOfView;
                Camera.main.nearClipPlane = cameraConfig.nearClipPlane;
                Camera.main.farClipPlane = cameraConfig.farClipPlane;
                Camera.main.depth = cameraConfig.depth;
                Camera.main.renderingPath = (RenderingPath)cameraConfig.renderingPath;
                Camera.main.useOcclusionCulling = cameraConfig.useOcclusionCulling;
                Camera.main.allowHDR = cameraConfig.allowHDR;
                Camera.main.allowMSAA = cameraConfig.allowMSAA;
                Camera.main.allowDynamicResolution = cameraConfig.allowDynamicResolution;
                Camera.main.targetDisplay = cameraConfig.targetDisplay;
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

            // Cursor
            Cursor.visible = cursorConfig.visible;
        }
    }
}
#endif