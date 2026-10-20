using RoR2;
using static LunarScrap.Content.LunarScrapItem;

namespace LunarScrap.Hooks;

internal static class ScrapTierHook
{
    internal static void Init()
    {
        On.RoR2.PickupCatalog.FindScrapIndexForItemTier += HandleLunarScrapping;
    }

    // Adds payout for lunar items
    private static PickupIndex HandleLunarScrapping(On.RoR2.PickupCatalog.orig_FindScrapIndexForItemTier orig, ItemTier tier)
    {
        if (tier == ItemTier.Lunar)
        {
            return PickupCatalog.FindPickupIndex(LunarScrapItemDef!.itemIndex);
        }

        return orig(tier);
    }
}
