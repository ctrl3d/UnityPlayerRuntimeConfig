using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace work.ctrl3d
{
    public static class RuntimeConfigManager
    {
        private static RuntimeConfigData _config;
        private static readonly string ConfigPath = Path.Combine(Application.streamingAssetsPath, "UnityPlayerRuntimeConfig.json");
        private static readonly string UserConfigPath = Path.Combine(Application.dataPath, "UnityPlayerRuntimeConfig.json");

        public static void Load()
        {
            var configPath = File.Exists(UserConfigPath) ? UserConfigPath : ConfigPath;
            
            if (File.Exists(configPath))
            {
                try
                {
                    var jsonContent = File.ReadAllText(configPath);
                    var jsonObject = JObject.Parse(jsonContent);
                    
                    _config = new RuntimeConfigData
                    {
                        // Unity 클래스 기반으로 섹션 파싱
                        application = ParseSection<ApplicationConfig>(jsonObject, "application") ?? new ApplicationConfig(),
                        screen = ParseSection<ScreenConfig>(jsonObject, "screen") ?? new ScreenConfig(),
                        quality = ParseSection<QualityConfig>(jsonObject, "quality") ?? new QualityConfig(),
                        audio = ParseSection<AudioConfig>(jsonObject, "audio") ?? new AudioConfig(),
                        camera = ParseSection<CameraConfig>(jsonObject, "camera") ?? new CameraConfig(),
                        window = ParseSection<WindowConfig>(jsonObject, "window") ?? new WindowConfig()
                    };
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"UnityPlayerRuntimeConfig.json 파일 로드 실패: {ex.Message}");
                    CreateDefaultConfig();
                }
            }
            else
            {
                CreateDefaultConfig();
            }
        }

        private static T ParseSection<T>(JObject jsonObject, string sectionName) where T : class
        {
            return jsonObject[sectionName]?.ToObject<T>();
        }

        private static void CreateDefaultConfig()
        {
            _config = new RuntimeConfigData
            {
                application = new ApplicationConfig(),
                screen = new ScreenConfig(),
                quality = new QualityConfig(),
                audio = new AudioConfig(),
                camera = new CameraConfig(),
                window = new WindowConfig()
            };

            Save();
        }

        private static void Save()
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(_config, Formatting.Indented);
                
                Directory.CreateDirectory(Path.GetDirectoryName(UserConfigPath)!);
                File.WriteAllText(UserConfigPath, jsonContent);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Unity config.json 파일 저장 실패: {ex.Message}");
            }
        }

        public static RuntimeConfigData GetConfig()
        {
            if (_config == null)
                Load();
            
            return _config;
        }

        // Unity 클래스별 접근자
        public static ApplicationConfig GetApplicationConfig() => GetConfig().application;
        public static ScreenConfig GetScreenConfig() => GetConfig().screen;
        public static QualityConfig GetQualityConfig() => GetConfig().quality;
        public static AudioConfig GetAudioConfig() => GetConfig().audio;
        public static CameraConfig GetCameraConfig() => GetConfig().camera;
        public static WindowConfig GetWindowConfig() => GetConfig().window;
    }
}
