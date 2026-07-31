using System.IO;
using System.Reflection;
using UnityEngine;

namespace LunarScrap.Utils;

internal static class AssetManagement
{
    private readonly static string _assetPath = "LunarScrap.Assets.";

    internal static Sprite LoadSpriteFromResources(string resourceName, float ppu = 50f)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_assetPath + resourceName);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);

        Texture2D tex = new(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(buffer, true);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;

        return Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f),
            ppu
        );
    }
}
