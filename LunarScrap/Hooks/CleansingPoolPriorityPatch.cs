using HarmonyLib;
using RoR2;
using static LunarScrap.Content.LunarScrapItem;

namespace LunarScrap.Hooks;

// Prioritizes lunar scrap over a random lunar
[HarmonyPatch(typeof(CostTypeCatalog.LunarItemOrEquipmentCostTypeHelper), nameof(CostTypeDef.PayCost))]
internal static class CleansingPoolPriorityPatch
{
    private static void Prefix(CostTypeDef.PayCostContext context, CostTypeDef.PayCostResults result)
    {
        Inventory inventory = context.activatorInventory;
        if (inventory == null) return;

        int scrapCount = inventory.GetItemCountPermanent(LunarScrapItemDef.itemIndex);
        if (scrapCount > 0)
        {
            var transform = new Inventory.ItemTransformation
            {
                allowWhenDisabled = true,
                forbidTempItems = true,
                minToTransform = 1,
                maxToTransform = 1,
                originalItemIndex = LunarScrapItemDef.itemIndex,
                newItemIndex = ItemIndex.None
            };

            // Remove the lunar scrap
            if (transform.TryTransform(inventory, out var tResult))
            {
                result.AddTakenItemsFromTransformation(in tResult);
                context.cost = 0;
                return;
            }
        }
    }
}
