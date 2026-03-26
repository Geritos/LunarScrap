using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LunarScrap.Compatibility;
using LunarScrap.Content;
using LunarScrap.Hooks;
using R2API.Utils;
using RoR2;

namespace LunarScrap;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency(R2API.ItemAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(R2API.LanguageAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(R2API.PrefabAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(LookingGlass.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(MoreStats.MoreStatsPlugin.guid, BepInDependency.DependencyFlags.SoftDependency)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
public class LunarScrapPlugin : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Geritos";
    public const string PluginName = "Lunar_Scrap";
    public const string PluginVersion = "1.0.0";

    private static ConfigFile LunarScrapConfigFile { get; set; }
    public static ConfigEntry<float> MultiplierConfig { get; set; }

    public void Awake()
    {
        Log.Init(Logger);

        HandleConfig();

        LunarScrapItem.Init();

        ScrapTierHook.Init();

        DrifterTinkerHook.Init();

        var harmony = new Harmony(PluginGUID);
        harmony.PatchAll();

        ApplyExtensions();
    }

    public void HandleConfig()
    {
        LunarScrapConfigFile = new ConfigFile(Paths.ConfigPath + $"\\{PluginGUID}.cfg", true);
        MultiplierConfig = Config.Bind(
            "Lunar Scrap Buff",
            "Trash To Tresure Blue Buff",
            0.05f,
            "Trash to Tresure (Drifter's passive) lunar scrap bonus luck per stack."
        );
    }

    public static void ApplyExtensions()
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
