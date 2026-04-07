using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using IL.RoR2;
using System.IO;
using UnityEngine;

namespace stormincursion
{
    #region Dependencies
    [BepInDependency("___riskofthunder.RoR2BepInExPack")]
    #endregion
    #region SoftDependencies
    [BepInDependency(LookingGlass.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    #endregion
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class stormincursionMain : BaseUnityPlugin
    {
        public const string GUID = "com.jdikwhat.stormincursion";
        public const string MODNAME = "Storm Incursion";
        public const string VERSION = "0.0.1";
        public static ManualLogSource logger { get; private set; }
        public static PluginInfo pluginInfo { get; private set; }
        public static stormincursionMain instance { get; private set; }
        internal static AssetBundle assetBundle { get; private set; }
        internal static string assetBundleDir => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pluginInfo.Location), "stormincursionAssets");
        public static bool isLookingGlassInstalled => Chainloader.PluginInfos.ContainsKey(LookingGlass.PluginInfo.PLUGIN_GUID);

        private void Awake()
        {
            instance = this;
            pluginInfo = Info;
            logger = Logger;
            new stormincursionContent();
        }
        internal static void LogFatal(object data)
        {
            instance.Logger.LogFatal(data);
        }
        internal static void LogError(object data)
        {
            instance.Logger.LogError(data);
        }
        internal static void LogWarning(object data)
        {
            instance.Logger.LogWarning(data);
        }
        internal static void LogMessage(object data)
        {
            instance.Logger.LogMessage(data);
        }
        internal static void LogInfo(object data)
        {
            instance.Logger.LogInfo(data);
        }
        internal static void LogDebug(object data)
        {
            instance.Logger.LogDebug(data);
        }
    }
}
