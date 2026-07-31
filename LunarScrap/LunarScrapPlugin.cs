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
[BepInDependency(LookingGlass.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(MoreStats.MoreStatsPlugin.guid, BepInDependency.DependencyFlags.SoftDependency)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
public class LunarScrapPlugin : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Geritos";
    public const string PluginName = "Lunar_Scrap";
    public const string PluginVersion = "1.0.2";
    internal static ConfigEntry<float> MultiplierConfig;

    public void Awake()
    {
        Log.Init(Logger);

        BindConfig();

        LunarScrapItem.Init();

        ScrapTierHook.Init();

        DrifterTinkerHook.Init();

        var harmony = new Harmony(PluginGUID);
        harmony.PatchAll();

        ApplyExtensions();
    }

    private void BindConfig()
    {
        MultiplierConfig = Config.Bind(
            "Lunar Scrap Buff",
            "Trash To Tresure Blue Buff",
            0.05f,
            "Trash to Tresure (Drifter's passive) lunar scrap bonus luck per stack."
        );
    }

    private static void ApplyExtensions()
    {
        if (MoreStatsCompatibility.Enabled)
        {
            MoreStatsCompatibility.ApplyTrashToTresureHook();

            if (LookingGlassCompatibility.Enabled)
            {
                LookingGlassCompatibility.LookingGlassPatch();
                RoR2Application.onLoad = () => 
                {
                    LookingGlassCompatibility.AddLunarScrapDescription();
                    LookingGlassCompatibility.AddLunarScrapBuffDescription();
                };
            }
        }
    }
}
