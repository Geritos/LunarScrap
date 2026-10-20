using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LunarScrap.Compatibility;
using LunarScrap.Content;
using LunarScrap.Hooks;
using R2API;
using R2API.Utils;
using RoR2;

namespace LunarScrap;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency(ItemAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(PrefabAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(LanguageAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("droppod.lookingglass", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.RiskOfBrainrot.MoreStats", BepInDependency.DependencyFlags.SoftDependency)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
public class LunarScrapPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "Geritos.Lunar_Scrap";
    public const string PluginAuthor = "Geritos";
    public const string PluginName = "Lunar_Scrap";
    public const string PluginVersion = "1.0.3";
    internal static ConfigEntry<float>? MultiplierConfig;

    public void Awake()
    {
        Log.Init(Logger);

        BindConfig();

        LunarScrapItem.Init();

        ScrapTierHook.Init();

        DrifterTinkerHook.Init();

        var harmony = new Harmony(PluginGUID);
        harmony.PatchAll(typeof(CleansingPoolPriorityPatch));
        harmony.PatchAll(typeof(ScrapperLunarDisplayPatch));

        ApplyExtensions();
    }

    private void BindConfig()
    {
        MultiplierConfig = Config.Bind(
            "Lunar Scrap Buff",
            "Trash To Treasure Blue Buff",
            0.05f,
            "Trash to Treasure (Drifter's passive) lunar scrap bonus luck per stack."
        );
    }

    private static void ApplyExtensions()
    {
        if (MoreStatsCompatibility.Enabled)
        {
            MoreStatsCompatibility.Init();

            if (LookingGlassCompatibility.Enabled)
            {
                LookingGlassCompatibility.LookingGlassPatch();
                RoR2Application.onLoad += () => 
                {
                    LookingGlassCompatibility.AddLunarScrapDescription();
                    LookingGlassCompatibility.AddLunarScrapBuffDescription();
                };
            }
        }
    }
}
