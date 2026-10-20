using BepInEx.Bootstrap;
using LookingGlass.ItemStatsNameSpace;
using LookingGlass.LookingGlassLanguage;
using MonoMod.RuntimeDetour;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using static LunarScrap.Content.LunarScrapItem;
using static LunarScrap.Content.TrashToTreasureBlue;

namespace LunarScrap.Compatibility;

internal static class LookingGlassCompatibility
{
    private static bool? _enabled;

    internal static bool Enabled
    {
        get
        {
            _enabled ??= Chainloader.PluginInfos.ContainsKey("droppod.lookingglass");
            return (bool)_enabled;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void AddLunarScrapBuffDescription()
    {
        var en = Language.languagesByName["en"];
        string t2tb = LunarScrapBuffDef!.name;
        LookingGlassLanguageAPI.SetupToken(en, "NAME_" + t2tb, $"Lunar Trash to Treasure");
        LookingGlassLanguageAPI.SetupToken(en, "DESCRIPTION_" + t2tb, $"Increase luck by {Multiplier}");
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void AddLunarScrapDescription()
    {
        ItemStatsDef itemStat = new();
        itemStat.descriptions.Add("Luck: ");
        itemStat.valueTypes.Add(ItemStatsDef.ValueType.Utility);
        itemStat.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Number);
        itemStat.isScrap = true;
        itemStat.calculateValues = (master, stackCount) =>
        {
            List<float> values = [Multiplier * stackCount];
            return values;
        };
        ItemDefinitions.allItemDefinitions.Add((int)LunarScrapItemDef!.itemIndex, itemStat);
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void LookingGlassPatch()
    {
        var asm = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == LookingGlass.PluginInfo.PLUGIN_NAME);
        var target = asm.GetType("LookingGlass.Utils")?.GetMethod(
            "CalculateChanceWithLuck",
            BindingFlags.Public | BindingFlags.Static
        );
        new Hook(
            target,
            typeof(LookingGlassCompatibility).GetMethod(
                nameof(TreatLuckAsFloatHook),
                BindingFlags.NonPublic | BindingFlags.Static
            )
        );
    }

    private static float TreatLuckAsFloatHook(Func<float, float, float> orig, float baseChance, float luckIn)
    {
        // Would reverse logic with float.IsIntegral but wrong .NET version, gg
        if (luckIn != Mathf.Floor(luckIn))
        {
            float floor = Mathf.Floor(baseChance);
            float frac = baseChance - floor;

            return Math.Sign(luckIn) switch
            {
                 1 => floor + 1f - Mathf.Pow(1f - frac, luckIn + 1f),
                -1 => floor + Mathf.Pow(frac, Mathf.Abs(luckIn) + 1f),
                _ => baseChance
            };
        }

        return orig(baseChance, luckIn);
    }
}
