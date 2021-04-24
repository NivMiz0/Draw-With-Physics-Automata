using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlaceCell : MonoBehaviour
{
    public Texture2D texture;

    public CellObject selectedCell;

    public int brushRad;
    byte[] bytes;

    private void Start()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if(x == texture.width - 1|| x == 0 || y == 0 || y == texture.height - 1)
                {
                    texture.SetPixel(x, y, Color.black);
                }
                else if (texture.GetPixel(x, y) != Color.white)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((int)Input.mouseScrollDelta.y != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    brushRad = Mathf.Clamp(brushRad - (int)Input.mouseScrollDelta.y, 1, 8);
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;

                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    texture.DrawCircle(selectedCell.color, (int)pixelUV.x, (int)pixelUV.y, brushRad);
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;

                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("UI"))
                {
                    texture.DrawCircle(Color.white, (int)pixelUV.x, (int)pixelUV.y, brushRad);
                }
                //print($"({(int)pixelUV.x}, {(int)pixelUV.y})" + texture.GetPixel((int)pixelUV.x, (int)pixelUV.y));
            }
        }

        /*if (Input.GetKeyDown("s") && (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl)))
        {
            bytes = texture.EncodeToPNG();
            PlayerPrefs.SetInt("NumSaved", PlayerPrefs.GetInt("NumSaved", 0) + 1);

            File.WriteAllBytes(Application.dataPath + $"/../SavedPixelWorld.png", bytes);
        }*/
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        fileData = File.ReadAllBytes(filePath);
        tex = new Texture2D(340, 180);
        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

        return tex;
    }
}

public static class Tex2DExtension
{
    public static Texture2D DrawCircle(this Texture2D tex, Color color, int x, int y, int radius = 3)
    {
        float rSquared = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    tex.SetPixel(u, v, color);

        return tex;
    }
    public static List<Color> GetCircle(this Texture2D tex, int x, int y, int radius = 3)
    {
        float rSquared = radius * radius;
        List<Color> colors = new List<Color>();

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    colors.Add(tex.GetPixel(u, v));

        return colors;
    }
}
