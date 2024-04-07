using UnityEngine;
using HMLLibrary;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

// This mod is a port of my BepInEx mod https://github.com/Nauja/raft-mods/tree/main/InfiniteDurability

public class InifiniteUse : Mod
{
    static JsonModInfo modInfo;

    Harmony harmony;

    private const string HARMONY_ID = "com.github.nauja.raftmods.infiniteuse";

    // Settings that toggle infinite items use
    public static HashSet<string> specials = new HashSet<string>();
    public static bool fluidInfiniteUse;
    public static bool foodInfiniteUse;
    public static bool usableInfiniteUse;
    public static bool equipmentInfiniteUse;

    public static new void Log(object message)
    {
        Debug.Log("[" + modInfo.name + "]: " + message.ToString());
    }

    public void Start()
    {
        modInfo = modlistEntry.jsonmodinfo;
        harmony = new Harmony(HARMONY_ID);
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Log("loaded [v" + modInfo.version + "].");
    }

    public void OnModUnload()
    {
        harmony.UnpatchAll(HARMONY_ID);
        Destroy(gameObject);
        Log("unloaded.");
    }

    /*******************************
    *   EXTRA-SETTINGS-API STUFF   *
    ********************************/
    static HarmonyLib.Traverse ExtraSettingsAPI_Traverse;
    static bool ExtraSettingsAPI_Loaded = false;

    public void ExtraSettingsAPI_Load()
    {
        ES_API_SetValues();
    }


    public void ExtraSettingsAPI_SettingsOpen()
    {
        ExtraSettingsAPI_SetCheckboxState("fluidInfiniteUse", fluidInfiniteUse);
        ExtraSettingsAPI_SetCheckboxState("foodInfiniteUse", foodInfiniteUse);
        ExtraSettingsAPI_SetCheckboxState("usableInfiniteUse", usableInfiniteUse);
        ExtraSettingsAPI_SetCheckboxState("equipmentInfiniteUse", equipmentInfiniteUse);
        ExtraSettingsAPI_SetInputValue("specialInfiniteUse", string.Join(", ", specials));
    }

    public void ExtraSettingsAPI_SettingsClose()
    {
        ES_API_SetValues();
    }

    public void ExtraSettingsAPI_SetCheckboxState(string SettingName, bool value)
    {
        if (ExtraSettingsAPI_Loaded)
            ExtraSettingsAPI_Traverse.Method("setCheckboxState", new object[] { this, SettingName, value }).GetValue<bool>();
    }

    public bool ExtraSettingsAPI_GetCheckboxState(string SettingName)
    {
        if (ExtraSettingsAPI_Loaded)
            return ExtraSettingsAPI_Traverse.Method("getCheckboxState", new object[] { this, SettingName }).GetValue<bool>();
        return false;
    }

    public void ExtraSettingsAPI_SetInputValue(string SettingName, string value)
    {
        if (ExtraSettingsAPI_Loaded)
            ExtraSettingsAPI_Traverse.Method("setInputValue", new object[] { this, SettingName, value }).GetValue<string>();
    }

    public string ExtraSettingsAPI_GetInputValue(string SettingName)
    {
        if (ExtraSettingsAPI_Loaded)
            return ExtraSettingsAPI_Traverse.Method("getInputValue", new object[] { this, SettingName }).GetValue<string>();
        return "";
    }

    public void ES_API_SetValues()
    {
        fluidInfiniteUse = ExtraSettingsAPI_GetCheckboxState("fluidInfiniteUse");
        foodInfiniteUse = ExtraSettingsAPI_GetCheckboxState("foodInfiniteUse");
        usableInfiniteUse = ExtraSettingsAPI_GetCheckboxState("usableInfiniteUse");
        equipmentInfiniteUse = ExtraSettingsAPI_GetCheckboxState("equipmentInfiniteUse");
        specials = ExtraSettingsAPI_GetInputValue("specialInfiniteUse").Split(',').Select(s => s.Trim()).ToHashSet();
    }

    public bool ExtraSettingsAPI_HandleSettingVisible(string SettingName)
    {
        return true;
    }

    /********************
    *   HARMONY STUFF   *
    *********************/
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.Uses))]
    [HarmonyPatch(MethodType.Getter)]
    static class armonyPatch_ItemInstance_Uses_Getter
    {
        static void Postfix(ItemInstance __instance, ref int __result)
        {
            var baseItem = __instance.baseItem;

            bool isInfiniteUseEnabled = false;
            if (specials.Contains(baseItem.UniqueName))
            {
                isInfiniteUseEnabled = true;
            }
            else if (baseItem.settings_consumeable.FoodType > FoodType.None)
            {
                switch (baseItem.settings_consumeable.FoodType)
                {
                    case FoodType.Food:
                        isInfiniteUseEnabled = foodInfiniteUse;
                        break;
                    case FoodType.Water:
                    case FoodType.SaltWater:
                        isInfiniteUseEnabled = fluidInfiniteUse;
                        break;
                }
            }
            else if (baseItem.settings_usable.IsUsable())
            {
                isInfiniteUseEnabled = usableInfiniteUse;
            }
            else if (baseItem.settings_equipment.EquipType > EquipSlotType.None)
            {
                isInfiniteUseEnabled = equipmentInfiniteUse;
            }

            if (isInfiniteUseEnabled)
            {
                __result = baseItem.MaxUses;
            }
        }
    }

}