using BepInEx.Bootstrap;
using LunarScrap.Content;
using LunarScrap.Hooks;
using RoR2;
using System.Runtime.CompilerServices;

namespace LunarScrap.Compatibility;

// JIT probably compiles static methods even if they are not called, therefore this class is not static.
// This prevents the "Could not load assembly" exception when MoreStats is not installed.
// It is possible that LookingGlass did not use custom event args structs or did not create
// compiler-generated static delegates caching fields in the same way C# delegates do.
// I am not even sure if this is a right way to approach this, but the only other way I can think of
// is to use reflection to get the event and add a delegate to it, which is probably worse.
internal class MoreStatsCompatibility
{
    private static MoreStatsCompatibility? _instance;
    internal static MoreStatsCompatibility Instance => _instance ??= new();

    private static bool? _enabled;

    internal static bool Enabled
    {
        get
        {
            _enabled ??= Chainloader.PluginInfos.ContainsKey("com.RiskOfBrainrot.MoreStats");
            return (bool)_enabled;
        }
    }

    internal static void Init()
    {
        if (Enabled)
        {
            Instance.ApplyTrashToTresureHook();
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void ApplyTrashToTresureHook()
    {
        TrashToTreasureBlue.Init();
        MoreStats.StatHooks.GetMoreStatCoefficients += ApplyStatBuffs;
        DrifterTrashToTreasureHook.Init();
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void ApplyStatBuffs(CharacterBody body, MoreStats.StatHooks.MoreStatHookEventArgs args)
    {
        if (body && body.HasBuff(TrashToTreasureBlue.LunarScrapBuffDef))
        {
            args.luckAdd += TrashToTreasureBlue.Multiplier * body.GetBuffCount(TrashToTreasureBlue.LunarScrapBuffDef);
        }
    }
}
