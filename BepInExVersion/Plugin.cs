using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using UnityEngine.UI;

namespace BepInExInfo;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class BepInExPlugin : BaseUnityPlugin
{
    private static BepInExPlugin context;

    public static ConfigEntry<bool> modEnabled;

    private void Awake()
    {
        context = this;
        modEnabled = Config.Bind<bool>("General", "ModEnabled", true, "Enable mod");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
    }

    [HarmonyPatch(typeof(StartMenuScreen), "Start")]
    [HarmonyPatch(MethodType.Normal)]
    static class StartMenuScreen_Start_Patch
    {
        static void Postfix(StartMenuScreen __instance)
        {
            if (!modEnabled.Value)
                return;

            ((Text)Traverse.Create(__instance).Field("versionText").GetValue()).text = $"{Settings.VersionNumberText} BepInEx {Chainloader.PluginInfos.Count} Plugins";
        }
    }
}
