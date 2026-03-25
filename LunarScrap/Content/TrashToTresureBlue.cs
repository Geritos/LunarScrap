using LunarScrap.Utils;
using R2API;
using RoR2;
using UnityEngine;

namespace LunarScrap.Content;

internal static class TrashToTresureBlue
{
    public static BuffDef LunarScrapBuffDef;

    public readonly static float Multiplier = LunarScrapPlugin.MultiplierConfig.Value;

    public static void Init()
    {
        LunarScrapBuffDef = ScriptableObject.CreateInstance<BuffDef>();
        LunarScrapBuffDef.name = "TrashToTresureBlue";
        LunarScrapBuffDef.canStack = true;
        LunarScrapBuffDef.isDebuff = false;
        LunarScrapBuffDef.ignoreGrowthNectar = false;
        LunarScrapBuffDef.isHidden = false;
        LunarScrapBuffDef.buffColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);
        LunarScrapBuffDef.iconSprite = AssetManagement.LoadSpriteFromResources("t2tb.png");
        ContentAddition.AddBuffDef(LunarScrapBuffDef);
    }
}
