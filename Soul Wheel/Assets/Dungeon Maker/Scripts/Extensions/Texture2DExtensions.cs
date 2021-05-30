using UnityEngine;

public static class Texture2DExtensions
{
    public static void ApplyColor(this Texture2D tex, Color col)
    {
        for (int m = 0; m < tex.mipmapCount; m++)
        {
            Color[] c = tex.GetPixels(m);
            for (int i = 0; i < c.Length; i++)
            {
                c[i].r = col.r;
                c[i].g = col.g;
                c[i].b = col.b;
            }
            tex.SetPixels(c, m);
        }
        tex.Apply();
    }
}