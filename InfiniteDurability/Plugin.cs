// Based on original plugin https://github.com/aedenthorn/RaftMods/blob/master/Durability/BepInExPlugin.cs
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InfiniteDurability;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class BepInExPlugin : BaseUnityPlugin
{
    private static BepInExPlugin context;

    public static ConfigEntry<bool> fluidInfiniteDurabilityEnabled;
    public static ConfigEntry<bool> foodInfiniteDurabilityEnabled;
    public static ConfigEntry<bool> usableInfiniteDurabilityEnabled;
    public static ConfigEntry<bool> equipmentInfiniteDurabilityEnabled;
    public static ConfigEntry<string> specialInfiniteDurabilityEnabled;
    public static ConfigEntry<bool> modEnabled;
    public static ConfigEntry<bool> isDebug;

    public static double lastTime = 1;
    public static bool pausedMenu = false;
    public static bool wasActive = false;
    public static Dictionary<string, bool> specials = new Dictionary<string, bool>();

    public static void Dbgl(string str = "", bool pref = true)
    {
        if (isDebug.Value)
            Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
    }

    private void Awake()
    {
        context = this;
        modEnabled = Config.Bind<bool>("General", "ModEnabled", true, "Enable mod");
        isDebug = Config.Bind<bool>("General", "IsDebug", false, "Enable debug");
        specialInfiniteDurabilityEnabled = Config.Bind<string>("Options", "SpecialInfiniteDurabilityEnabled", "", "Special items have infinite durability. Use format ItemName, e.g. HeadLight (comma-separated).");
        fluidInfiniteDurabilityEnabled = Config.Bind<bool>("Options", "FluidInfiniteDurabilityEnabled", false, "Fluid items have infinite durability");
        foodInfiniteDurabilityEnabled = Config.Bind<bool>("Options", "FoodInfiniteDurabilityEnabled", false, "Food items have infinite durability");
        usableInfiniteDurabilityEnabled = Config.Bind<bool>("Options", "UsableInfiniteDurabilityEnabled", false, "Usable items have infinite durability");
        equipmentInfiniteDurabilityEnabled = Config.Bind<bool>("Options", "EquipmentInfiniteDurabilityEnabled", false, "Equipment items have infinite durability");
        Dbgl($"SpecialInfiniteDurabilityEnabled={specialInfiniteDurabilityEnabled.Value}");
        Dbgl($"FluidInfiniteDurabilityEnabled={fluidInfiniteDurabilityEnabled.Value}");
        Dbgl($"FoodInfiniteDurabilityEnabled={foodInfiniteDurabilityEnabled.Value}");
        Dbgl($"UsableInfiniteDurabilityEnabled={usableInfiniteDurabilityEnabled.Value}");
        Dbgl($"EquipmentInfiniteDurabilityEnabled={equipmentInfiniteDurabilityEnabled.Value}");

        var array = specialInfiniteDurabilityEnabled.Value.Split(',');
        foreach (var s in array)
        {
            specials.Add(s, true);
        }
        Dbgl($"Got {specials.Count} special mults");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
    }

    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.Uses))]
    [HarmonyPatch(MethodType.Getter)]
    static class ItemInstance_Uses_Getter_Patch
    {
        static void Postfix(ItemInstance __instance, ref int __result)
        {
            if (!modEnabled.Value)
                return;

            var baseItem = __instance.baseItem;

            string category = "unknown";
            bool isInfiniteDurabilityEnabled = false;
            if (specials.TryGetValue(baseItem.UniqueName, out isInfiniteDurabilityEnabled))
            {
                category = "special";
            }
            else if (baseItem.settings_consumeable.FoodType > FoodType.None)
            {
                switch (baseItem.settings_consumeable.FoodType)
                {
                    case FoodType.Food:
                        isInfiniteDurabilityEnabled = foodInfiniteDurabilityEnabled.Value;
                        break;
                    case FoodType.Water:
                    case FoodType.SaltWater:
                        isInfiniteDurabilityEnabled = fluidInfiniteDurabilityEnabled.Value;
                        break;
                }
                category = "consumable";
            }
            else if (baseItem.settings_usable.IsUsable())
            {
                isInfiniteDurabilityEnabled = usableInfiniteDurabilityEnabled.Value;
                //category = "tool";
            }
            else if (baseItem.settings_equipment.EquipType > EquipSlotType.None)
            {
                isInfiniteDurabilityEnabled = equipmentInfiniteDurabilityEnabled.Value;
                category = "equipment";
            }

            if (isInfiniteDurabilityEnabled)
            {
                Dbgl($"{category} {baseItem.UniqueName}; infinite durability enabled");
                __result = baseItem.MaxUses;
            }
        }
    }
}
