using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Directions
{
    down,
    downRight,
    downLeft,
    up,
    upRight,
    upLeft,
    right,
    left
}
public class SimulateCells : MonoBehaviour
{
    public Texture2D texture;
    public float updateSpeed;
    public bool doTheThing;

    Color pixel;

    Color down;
    Color downLeft;
    Color downRight;
    Color up;
    Color upLeft;
    Color upRight;
    Color left;
    Color right;

    int texCoordX;
    int texCoordY;

    //Directions dir;

    // Update is called once per frame
    void Update()
    {
        texture.Apply();
    }

    private IEnumerator Start()
    {
        while (doTheThing)
        {
            yield return new WaitForSeconds(updateSpeed);
            UpdateCells();
        }
    }

    void UpdateCells()
    {
        List<Vector2> banned = new List<Vector2>();

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texCoordX = x;
                texCoordY = y;
                Vector2 isBanned = new Vector2(texCoordX, texCoordY);
                if (CellTypeDatabase.instance.colors.Contains(texture.GetPixel(texCoordX, texCoordY)) && x != texture.width - 1 && x != 0 && y != 0 && y != texture.height - 1 && !banned.Contains(isBanned))
                {
                    CellObject cell = CellTypeDatabase.instance.cellObjects[CellTypeDatabase.instance.colors.IndexOf(texture.GetPixel(texCoordX, texCoordY))];
                    pixel = texture.GetPixel(texCoordX, texCoordY);

                    down = texture.GetPixel(texCoordX, texCoordY - 1);
                    downLeft = texture.GetPixel(texCoordX - 1, texCoordY - 1);
                    downRight = texture.GetPixel(texCoordX + 1, texCoordY - 1);
                    up = texture.GetPixel(texCoordX, texCoordY + 1);
                    upLeft = texture.GetPixel(texCoordX - 1, texCoordY + 1);
                    upRight = texture.GetPixel(texCoordX + 1, texCoordY + 1);
                    left = texture.GetPixel(texCoordX - 1, texCoordY);
                    right = texture.GetPixel(texCoordX + 1, texCoordY);

                    if (cell.useRigidGravity)
                    {
                        if (down == Color.white || CellTypeDatabase.instance.liquids.Contains(down) || CellTypeDatabase.instance.gasses.Contains(down))
                        {
                            texture.SetPixel(texCoordX, texCoordY, down);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                    }
                    else if (cell.usePowderGravity)
                    {
                        if (down == Color.white || CellTypeDatabase.instance.liquids.Contains(down) || CellTypeDatabase.instance.gasses.Contains(down))
                        {
                            texture.SetPixel(texCoordX, texCoordY, down);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                        else if (downLeft == Color.white || CellTypeDatabase.instance.liquids.Contains(downLeft) || CellTypeDatabase.instance.gasses.Contains(downLeft))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downLeft);
                            texture.SetPixel(texCoordX - 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX - 1, texCoordY - 1));
                        }
                        else if (downRight == Color.white || CellTypeDatabase.instance.liquids.Contains(downRight) || CellTypeDatabase.instance.gasses.Contains(downRight))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downRight);
                            texture.SetPixel(texCoordX + 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX + 1, texCoordY - 1));
                        }
                    }
                    else if (cell.useGasPowderGravity)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.floatSlowMod);
                        if (rnd == 1)
                        {
                            if (up == Color.white || CellTypeDatabase.instance.liquids.Contains(up))
                            {
                                texture.SetPixel(texCoordX, texCoordY, up);
                                texture.SetPixel(texCoordX, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX, texCoordY + 1));
                            }
                            else if (upLeft == Color.white || CellTypeDatabase.instance.liquids.Contains(upLeft))
                            {
                                texture.SetPixel(texCoordX, texCoordY, upLeft);
                                texture.SetPixel(texCoordX - 1, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX - 1, texCoordY + 1));
                            }
                            else if (upRight == Color.white || CellTypeDatabase.instance.liquids.Contains(upRight))
                            {
                                texture.SetPixel(texCoordX, texCoordY, upRight);
                                texture.SetPixel(texCoordX + 1, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX + 1, texCoordY + 1));
                            }
                        }
                    }
                    else if (cell.useLiquidGravity)
                    {
                        if (down == Color.white)
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                        }
                        else if (downRight == Color.white)
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX + 1, texCoordY - 1, pixel);
                        }
                        else if (downLeft == Color.white)
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX - 1, texCoordY - 1, pixel);
                        }
                        else if (right == Color.white)
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX + 1, texCoordY, pixel);
                        }
                        else if (left == Color.white)
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX - 1, texCoordY, pixel);
                        }
                    }
                    else
                    {
                        continue;
                    }
                    if (cell.corrode)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.corrodeSlowMod);
                        if(rnd == 1 && CellTypeDatabase.instance.solids.Contains(down) && CellTypeDatabase.instance.cellObjects[CellTypeDatabase.instance.solids.IndexOf(down)].destructible)
                        {
                            texture.SetPixel(texCoordX, texCoordY - 1, Color.white);
                        }
                    }
                    if (cell.harden)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.hardenSlowMod);
                        if (rnd == 1 && (CellTypeDatabase.instance.liquids.Contains(up) || CellTypeDatabase.instance.liquids.Contains(left) || CellTypeDatabase.instance.liquids.Contains(right) || up == cell.hardenInto.color || right == cell.hardenInto.color || left == cell.hardenInto.color || down == cell.hardenInto.color))
                        {
                            texture.SetPixel(texCoordX, texCoordY, cell.hardenInto.color);
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }
}