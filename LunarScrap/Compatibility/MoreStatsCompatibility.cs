using BepInEx.Bootstrap;
using LunarScrap.Content;
using LunarScrap.Hooks;
using System.Runtime.CompilerServices;

namespace LunarScrap.Compatibility;

internal static class MoreStatsCompatibility
{
    private static bool? _enabled;

    internal static bool Enabled
    {
        get
        {
            _enabled ??= Chainloader.PluginInfos.ContainsKey(MoreStats.MoreStatsPlugin.guid);
            return (bool)_enabled;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void ApplyTrashToTresureHook()
    {
        TrashToTreasureBlue.Init();
        DrifterTrashToTreasureHook.Init();
    }
}
