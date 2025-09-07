using System;
using UnityEngine;

namespace work.ctrl3d
{
    [Serializable]
    public class RuntimeConfig
    {
        public ApplicationConfig application = new();
        public ScreenConfig screen = new();
        public QualityConfig quality = new();
        public AudioConfig audio = new();
        public CameraConfig camera = new();
        public WindowConfig window = new();
        public CursorConfig cursor = new();
    }

    [Serializable]
    public class ApplicationConfig
    {
        public bool runInBackground = true;
        public int targetFrameRate = 60;
        public int backgroundLoadingPriority = 2;
    }

    [Serializable]
    public class ScreenConfig
    {
        public int x;
        public int y;
        public int width = 1280;
        public int height = 720;
        public bool fullScreen;
    }

    [Serializable]
    public class QualityConfig
    {
        public int anisotropicFiltering = 2;
        public int vSyncCount;
        public int qualityLevel = 5;
        public int antiAliasing;
    }

    [Serializable]
    public class AudioConfig
    {
        public float volume = 1.0f;
    }

    [Serializable]
    public class CameraConfig
    {
        public int clearFlags = 2;
        public string backgroundColor = "#000000";
        public bool orthographic;
        public float orthographicSize = 5.0f;
        public float fieldOfView = 60.0f;
        public float nearClipPlane = 0.3f;
        public float farClipPlane = 1000.0f;
        public float depth = -1f;
        public int renderingPath = -1;
        public bool useOcclusionCulling = true;
        public bool allowHDR = true;
        public bool allowMSAA = true;
        public bool allowDynamicResolution;
        public int targetDisplay;
    }

    [Serializable]
    public class WindowConfig
    {
        public bool popupWindow;
        public string zOrder = "TOP";
    }

    [Serializable]
    public class CursorConfig
    {
        public bool visible;
    }
}