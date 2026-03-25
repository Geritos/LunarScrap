using BepInEx.Bootstrap;
using LunarScrap.Content;
using LunarScrap.Hooks;
using System.Runtime.CompilerServices;

namespace LunarScrap.Compatibility;

public static class MoreStatsCompatibility
{
    private static bool? _enabled;

    public static bool Enabled
    {
        get
        {
            _enabled ??= Chainloader.PluginInfos.ContainsKey(MoreStats.MoreStatsPlugin.guid);
            return (bool)_enabled;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void ApplyTrashToTresureHook()
    {
        TrashToTresureBlue.Init();
        DrifterTrashToTresureHook.Init();
    }
}
