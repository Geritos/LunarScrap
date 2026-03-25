using HarmonyLib;
using RoR2;
using System.Collections.Generic;
using static LunarScrap.Content.LunarScrapItem;

namespace LunarScrap.Hooks;

// Adds lunar items to the scrapper UI
[HarmonyPatch(typeof(ScrapperController), nameof(ScrapperController.AssignPotentialInteractor))]
internal static class ScrapperLunarDisplayPatch
{
    private static void Postfix(ScrapperController __instance, Interactor potentialInteractor)
    {
        // Since this is postfix, the checks for the existence of these should already be done
        PickupPickerController picker = __instance.pickupPickerController;
        CharacterBody body = potentialInteractor.GetComponent<CharacterBody>();
        Inventory inv = body.inventory;
        

        List<PickupPickerController.Option> lunarOptions = [];
        // Collect the lunar options for scrapper
        foreach (ItemIndex itemIndex in ItemCatalog.lunarItemList)
        {
            ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
            if (itemDef.hidden || !itemDef.canRemove || itemDef == LunarScrapItemDef) continue;
            if (inv.GetItemCountPermanent(itemDef.itemIndex) == 0) continue;

            PickupIndex pickupIndex = PickupCatalog.FindPickupIndex(itemIndex);
            lunarOptions.Add(new PickupPickerController.Option
            {
                available = true,
                pickup = new UniquePickup() { decayValue = 0f, pickupIndex = pickupIndex }
            });
        }

        // Add options to scrapper
        PickupPickerController.Option[] combined = new PickupPickerController.Option[picker.options.Length + lunarOptions.Count];
        picker.options.CopyTo(combined, 0);
        lunarOptions.CopyTo(combined, picker.options.Length);

        picker.SetOptionsServer(combined);
    }
}