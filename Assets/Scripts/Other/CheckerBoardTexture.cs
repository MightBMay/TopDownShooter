using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerBoardTexture : MonoBehaviour
{
    public int textureSize = 256;
    public Color color1 = Color.white;
    public Color color2 = Color.black;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        var texture = new Texture2D(textureSize, textureSize);
        var pixels = new Color[textureSize * textureSize];

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                var color = ((x & 8) == (y & 8)) ? color1 : color2;
                pixels[y * textureSize + x] = color;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        rend.material.SetFloat("_Glossiness", 1f);
        rend.material.SetFloat("_GlossMapScale", 1f);
        GetComponent<Renderer>().material.mainTexture = texture;
    }
}
