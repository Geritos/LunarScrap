using R2API;
using RoR2;
using UnityEngine;
using LunarScrap.Utils;

namespace LunarScrap.Content;

public static class LunarScrapItem
{
    public static ItemDef LunarScrapItemDef;

    internal static void Init()
    {
        LunarScrapItemDef = ScriptableObject.CreateInstance<ItemDef>();
        LunarScrapItemDef.name = "ScrapBlue";
        string internalName = LunarScrapItemDef.name.ToUpperInvariant();
        LunarScrapItemDef.nameToken = "ITEM_" + internalName + "_NAME";
        LunarScrapItemDef.loreToken = "ITEM_" + internalName + "_LORE";
        LunarScrapItemDef.pickupToken = "ITEM_" + internalName + "_PICKUP";
        LunarScrapItemDef.descriptionToken = "ITEM_" + internalName + "_DESC";

        LunarScrapItemDef.tier = ItemTier.Lunar; // probably unnecessary
        LunarScrapItemDef.deprecatedTier = ItemTier.Lunar;
        LunarScrapItemDef.tags = [ItemTag.Scrap, ItemTag.WorldUnique, ItemTag.Cleansable, ItemTag.AIBlacklist]; // The cleansable tag is not used afaik
        LunarScrapItemDef.canRemove = true;
        LunarScrapItemDef.hidden = false;

        // Vanilla scrap items use a 128x128 texture with 25 PPU
        LunarScrapItemDef.pickupIconSprite = AssetManagement.LoadSpriteFromResources("iconBlueScrap.png", 25);
        LunarScrapItemDef.pickupModelPrefab = CreateLunarScrapPickupModel();

        ItemAPI.Add(new CustomItem(LunarScrapItemDef, new ItemDisplayRuleDict(null)));

        LanguageAPI.Add("ITEM_" + internalName + "_NAME", "Item Scrap, Blue");
        LanguageAPI.Add("ITEM_" + internalName + "_LORE", "While officially licensed materials are always recommended, hobbyists are known to take matters into their own hands when in a pinch. Don’t have any materials up to par? Make your own! Recycling old parts can be a fun way to create materials for your projects.\r\n- The Ultimate Hobby Guide: Kitbashing and 3D Printing");
        LanguageAPI.Add("ITEM_" + internalName + "_PICKUP", "Does nothing. Prioritized when used with Cleansing Pools.");
        LanguageAPI.Add("ITEM_" + internalName + "_DESC", "Does nothing. Prioritized when used with Cleansing Pools.");
    }

    internal static GameObject CreateLunarScrapPickupModel()
    {
        ItemDef baseScrap = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ScrapWhite");
        return PrefabAPI.InstantiateClone(baseScrap.pickupModelPrefab, "PickupBlueScrap", false);
    }
}
