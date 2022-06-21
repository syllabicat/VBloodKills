using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using Wetstone.API;
using BepInEx.Logging;
using HarmonyLib;

namespace VBloodKills
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("xyz.molenzwiebel.wetstone")]
    [Reloadable]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Logger;
        private Harmony _harmony;
        public override void Load()
        {
            if (!VWorld.IsServer) return;
            Logger = Log;
            _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
        public override bool Unload()
        {
            if (!VWorld.IsServer) return true;
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is unloaded!");
            return true;
        }
    }
}
