using System;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace work.ctrl3d.UnityPlayerRuntimeConfig
{
    [InitializeOnLoad]
    public class PackageInstaller
    {
        private const string WinUtilName = "work.ctrl3d.win-util";
        private const string WinUtilGitUrl = "https://github.com/ctrl3d/WinUtil.git?path=Assets/WinUtil";

        private const string JsonConfigName = "work.ctrl3d.json-config";
        private const string JsonConfigGitUrl = "https://github.com/ctrl3d/JsonConfig.git?path=Assets/JsonConfig";
        
        static PackageInstaller()
        {
            var isWinUtilInstalled = CheckPackageInstalled(WinUtilName);
            if (!isWinUtilInstalled) AddGitPackage(WinUtilName, WinUtilGitUrl);
            
            var isJsonConfigInstalled = CheckPackageInstalled(JsonConfigName);
            if (!isJsonConfigInstalled) AddGitPackage(JsonConfigName, JsonConfigGitUrl);
        }
        
        private static void AddGitPackage(string packageName, string gitUrl)
        {
            var path = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            var jsonString = File.ReadAllText(path);

            var indexOfLastBracket = jsonString.IndexOf("}", StringComparison.Ordinal);
            var dependenciesSubstring = jsonString[..indexOfLastBracket];
            var endOfLastPackage = dependenciesSubstring.LastIndexOf("\"", StringComparison.Ordinal);

            jsonString = jsonString.Insert(endOfLastPackage + 1, $", \n \"{packageName}\": \"{gitUrl}\"");

            File.WriteAllText(path, jsonString);
            Client.Resolve();
        }

        private static bool CheckPackageInstalled(string packageName)
        {
            var path = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            var jsonString = File.ReadAllText(path);
            return jsonString.Contains(packageName);
        }
    }
}