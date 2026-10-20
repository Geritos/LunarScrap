using R2API;
using RoR2;
using static LunarScrap.Content.LunarScrapItem;
using static LunarScrap.Content.TrashToTreasureBlue;

namespace LunarScrap.Hooks;

internal static class DrifterTrashToTreasureHook
{
    internal static void Init()
    {
        On.RoR2.DrifterTrashToTreasureController.OnInventoryChanged += SetLunarScrapBuff;
        On.RoR2.DrifterTrashToTreasureController.UpdateBuffCounts += SetLunarScrapBuff;
        On.RoR2.DrifterTrashToTreasureController.ResetBuffCounts += ResetLunarScrapBuff;
        On.RoR2.Language.SetCurrentLanguage += AppendRecycleDescription;
    }

    // MoreStats sets the scrap buffs here unlike vanilla...
    private static void SetLunarScrapBuff(On.RoR2.DrifterTrashToTreasureController.orig_OnInventoryChanged orig, DrifterTrashToTreasureController self)
    {
        orig(self);
        self.body.SetBuffCount(LunarScrapBuffDef!.buffIndex, self.body.inventory.GetItemCountEffective(LunarScrapItemDef));
    }

    // This is where vanilla sets the scrap buffs
    private static void SetLunarScrapBuff(On.RoR2.DrifterTrashToTreasureController.orig_UpdateBuffCounts orig, DrifterTrashToTreasureController self)
    {
        orig(self);
        self.body.SetBuffCount(LunarScrapBuffDef!.buffIndex, self.body.inventory.GetItemCountEffective(LunarScrapItemDef));
    }

    private static void ResetLunarScrapBuff(On.RoR2.DrifterTrashToTreasureController.orig_ResetBuffCounts orig, DrifterTrashToTreasureController self)
    {
        orig(self);
        self.body.SetBuffCount(LunarScrapBuffDef!.buffIndex, 0);
    }

    private static void AppendRecycleDescription(On.RoR2.Language.orig_SetCurrentLanguage orig, string newCurrentLanguageName, bool forceUnloadCurrentLanguage)
    {
        orig(newCurrentLanguageName, forceUnloadCurrentLanguage);
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
