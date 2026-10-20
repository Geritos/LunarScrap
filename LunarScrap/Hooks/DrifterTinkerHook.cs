using static LunarScrap.Content.LunarScrapItem;
using RoR2;

namespace LunarScrap.Hooks;

internal static class DrifterTinkerHook
{
    internal static void Init()
    {
        On.RoR2.DrifterTracker.IsWhitelist += HandleLunarTinker;
        On.RoR2.Util.RollTemporaryItemFromItemIndex += HandleLunarToTemporary;
    }

    private static ItemIndex HandleLunarToTemporary(On.RoR2.Util.orig_RollTemporaryItemFromItemIndex orig, ItemIndex itemIndex)
    {
        // The scrap check is probably not necessary because its handled by IsWhitelist
        if (PickupCatalog.FindPickupIndex(itemIndex).pickupDef.isLunar && itemIndex != LunarScrapItemDef!.itemIndex)
        {
            return itemIndex;
        }
        return orig(itemIndex);
    }

    private static bool HandleLunarTinker(On.RoR2.DrifterTracker.orig_IsWhitelist orig, DrifterTracker self, ref UniquePickup pickup)
    {
        PickupDef pickupDef = PickupCatalog.GetPickupDef(pickup.pickupIndex);
        if (pickupDef.itemTier == ItemTier.Lunar && pickup.pickupIndex.itemIndex != LunarScrapItemDef!.itemIndex)
        {
            return true;
        }

        return orig(self, ref pickup);
    }
}
