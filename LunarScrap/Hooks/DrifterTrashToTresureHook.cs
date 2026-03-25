using R2API;
using RoR2;
using static LunarScrap.Content.LunarScrapItem;
using static LunarScrap.Content.TrashToTresureBlue;
using static MoreStats.StatHooks;

namespace LunarScrap.Hooks;

internal static class DrifterTrashToTresureHook
{
    public static void Init()
    {
        GetMoreStatCoefficients += ApplyStatBuffs;
        On.RoR2.DrifterTrashToTreasureController.OnInventoryChanged += SetLunarScrapBuff;
        On.RoR2.DrifterTrashToTreasureController.UpdateBuffCounts += SetLunarScrapBuff;
        On.RoR2.DrifterTrashToTreasureController.ResetBuffCounts += ResetLunarScrapBuff;
        On.RoR2.Language.SetCurrentLanguage += AppendRecycleDescription;
    }

    public static void ApplyStatBuffs(CharacterBody body, MoreStatHookEventArgs args)
    {
        if (body && body.HasBuff(LunarScrapBuffDef))
        {
            Log.Debug($"Has buff, adding: {Multiplier * body.GetBuffCount(LunarScrapBuffDef)} luck");
            args.luckAdd += Multiplier * body.GetBuffCount(LunarScrapBuffDef);
        }
    }


    // MoreStats sets the scrap buffs here unlike vanilla...
    private static void SetLunarScrapBuff(On.RoR2.DrifterTrashToTreasureController.orig_OnInventoryChanged orig, DrifterTrashToTreasureController self)
    {
        orig(self);
        self.body.SetBuffCount(LunarScrapBuffDef.buffIndex, self.body.inventory.GetItemCountEffective(LunarScrapItemDef));
    }

    // This is where vanilla sets the scrap buffs
    private static void SetLunarScrapBuff(On.RoR2.DrifterTrashToTreasureController.orig_UpdateBuffCounts orig, DrifterTrashToTreasureController self)
    {
        orig(self);
        self.body.SetBuffCount(LunarScrapBuffDef.buffIndex, self.body.inventory.GetItemCountEffective(LunarScrapItemDef));
    }

    private static void ResetLunarScrapBuff(On.RoR2.DrifterTrashToTreasureController.orig_ResetBuffCounts orig, DrifterTrashToTreasureController self)
    {
        orig(self);
        self.body.SetBuffCount(LunarScrapBuffDef.buffIndex, 0);
    }

    private static void AppendRecycleDescription(On.RoR2.Language.orig_SetCurrentLanguage orig, string newCurrentLanguageName)
    {
        orig(newCurrentLanguageName);
        const string Token = "DRIFTER_RECYCLE_TOOLTIP";
        const string FormatEnd = "</style>";

        string tooltip = Language.GetString(Token);
        int instertPosition = tooltip.Length;
        if (tooltip.EndsWith(FormatEnd))
        {
            instertPosition -= FormatEnd.Length;
        }

        tooltip = tooltip.Insert(instertPosition, $"\r\nLunar - <style=cIsUtility>+{Multiplier} luck</style> <style=cStack>(per stack)</style>.");
        LanguageAPI.Add(Token, tooltip);
    }
}
