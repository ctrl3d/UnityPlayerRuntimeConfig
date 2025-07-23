using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEngine;

namespace work.ctrl3d.UnityPlayerRuntimeConfig
{
    [InitializeOnLoad]
    public class PackageInstaller
    {
        private const string WinUtilName = "work.ctrl3d.win-util";
        private const string WinUtilGitUrl = "https://github.com/ctrl3d/WinUtil.git?path=Assets/WinUtil";
        
        static PackageInstaller()
        {
            var isWinUtilInstalled = CheckPackageInstalled(WinUtilName);
            if (!isWinUtilInstalled) AddGitPackage(WinUtilName, WinUtilGitUrl);
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
        
        private static void AddScriptingDefineSymbol(string symbol)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            
            var symbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            if (!symbols.Contains(symbol))
            {
                symbols += $";{symbol}";
            }
            
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, symbols);
        }
        
        private static bool HasScriptingDefineSymbol(string symbol)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            
            var symbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            var symbolArray = symbols.Split(';');
            return symbolArray.Any(existingSymbol => existingSymbol == symbol);
        }
    }
}