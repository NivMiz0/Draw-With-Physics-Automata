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

    // Update is called once per frame
    void Update()
    {
        texture.Apply();
    }

    private IEnumerator Start()
    {
        int frameCount = 0;
        while (doTheThing)
        {
            yield return new WaitForSeconds(updateSpeed);
            frameCount++;
            if(frameCount % 2 == 0)
            {
                UpdateCellsEven();
            }
            else
            {
                UpdateCellsOdd();
            }
               
        }
    }

    void UpdateCellsEven()
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
                    CellObject cell = GetCellObject(texture.GetPixel(texCoordX, texCoordY));
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
                        if (down == Color.white || CellTypeDatabase.instance.liquids.Contains(down) || cell.goThrough.Contains(GetCellObject(down)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, down);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                    }
                    else if (cell.usePowderGravity)
                    {
                        if (down == Color.white || CellTypeDatabase.instance.liquids.Contains(down) || cell.goThrough.Contains(GetCellObject(down)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, down);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                        else if (downLeft == Color.white || CellTypeDatabase.instance.liquids.Contains(downLeft) || cell.goThrough.Contains(GetCellObject(downLeft)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downLeft);
                            texture.SetPixel(texCoordX - 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX - 1, texCoordY - 1));
                        }
                        else if (downRight == Color.white || CellTypeDatabase.instance.liquids.Contains(downRight) || cell.goThrough.Contains(GetCellObject(downRight)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downRight);
                            texture.SetPixel(texCoordX + 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX + 1, texCoordY - 1));
                        }
                    }
                    else if (cell.useGasGravity)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.floatSlowMod);
                        if (rnd == 1)
                        {
                            if (up == Color.white || cell.goThrough.Contains(GetCellObject(up)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, up);
                                texture.SetPixel(texCoordX, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX, texCoordY + 1));
                            }
                            else if (upRight == Color.white || cell.goThrough.Contains(GetCellObject(upRight)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, upRight);
                                texture.SetPixel(texCoordX + 1, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX + 1, texCoordY + 1));
                            }
                            else if (upLeft == Color.white || cell.goThrough.Contains(GetCellObject(upLeft)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, upLeft);
                                texture.SetPixel(texCoordX - 1, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX - 1, texCoordY + 1));
                            }
                            else if (right == Color.white || cell.goThrough.Contains(GetCellObject(right)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, right);
                                texture.SetPixel(texCoordX + 1, texCoordY, pixel);
                                banned.Add(new Vector2(texCoordX + 1, texCoordY));
                            }
                            else if(left == Color.white || cell.goThrough.Contains(GetCellObject(left)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, left);
                                texture.SetPixel(texCoordX - 1, texCoordY, pixel);
                                banned.Add(new Vector2(texCoordX - 1, texCoordY));
                            }
                        }
                    }
                    else if (cell.useLiquidGravity)
                    {
                        if (down == Color.white || cell.goThrough.Contains(GetCellObject(down)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                        else if (downLeft == Color.white || cell.goThrough.Contains(GetCellObject(downLeft)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX - 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX - 1, texCoordY - 1));
                        }
                        else if (downRight == Color.white || cell.goThrough.Contains(GetCellObject(downRight)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX + 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX + 1, texCoordY - 1));
                        }
                        else if (left == Color.white || cell.goThrough.Contains(GetCellObject(left)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX - 1, texCoordY, pixel);
                            banned.Add(new Vector2(texCoordX - 1, texCoordY));
                        }
                        else if (right == Color.white || cell.goThrough.Contains(GetCellObject(right)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, Color.white);
                            texture.SetPixel(texCoordX + 1, texCoordY, pixel);
                            banned.Add(new Vector2(texCoordX + 1, texCoordY));
                        }
                    }
                    else if (cell.isStatic) { }
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
                    if (cell.morphOnCollision)
                    {
                        int rnd;

                        for (int i = 0; i < cell.morphCollision.Length; i++)
                        {
                            rnd = Random.Range(1, cell.morphSlowMod[i]);
                            if (rnd == 1 && (down == cell.morphCollision[i].color || up == cell.morphCollision[i].color || right == cell.morphCollision[i].color || left == cell.morphCollision[i].color))
                            {
                                texture.SetPixel(texCoordX, texCoordY, cell.morphInto[i].color);
                            }
                        }
                    }
                    if (cell.selfDecay)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.decaySlowMod);
                        if (rnd == 1)
                        {
                            if(cell.decayInto == null)
                            {
                                texture.SetPixel(texCoordX, texCoordY, Color.white);
                            }
                            else
                            {
                                texture.SetPixel(texCoordX, texCoordY, cell.decayInto.color);
                            }
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

    void UpdateCellsOdd()
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
                    CellObject cell = GetCellObject(texture.GetPixel(texCoordX, texCoordY));
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
                        if (down == Color.white || CellTypeDatabase.instance.liquids.Contains(down) || cell.goThrough.Contains(GetCellObject(down)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, down);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                    }
                    else if (cell.usePowderGravity)
                    {
                        if (down == Color.white || CellTypeDatabase.instance.liquids.Contains(down) || cell.goThrough.Contains(GetCellObject(down)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, down);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                        else if (downRight == Color.white || CellTypeDatabase.instance.liquids.Contains(downRight) || cell.goThrough.Contains(GetCellObject(downRight)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downRight);
                            texture.SetPixel(texCoordX + 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX + 1, texCoordY - 1));
                        }
                        else if (downLeft == Color.white || CellTypeDatabase.instance.liquids.Contains(downLeft) || cell.goThrough.Contains(GetCellObject(downLeft)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downLeft);
                            texture.SetPixel(texCoordX - 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX - 1, texCoordY - 1));
                        }
                    }
                    else if (cell.useGasGravity)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.floatSlowMod);
                        if (rnd == 1)
                        {
                            if (up == Color.white || cell.goThrough.Contains(GetCellObject(up)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, up);
                                texture.SetPixel(texCoordX, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX, texCoordY + 1));
                            }
                            else if (upLeft == Color.white || cell.goThrough.Contains(GetCellObject(upLeft)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, upLeft);
                                texture.SetPixel(texCoordX - 1, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX - 1, texCoordY + 1));
                            }
                            else if (upRight == Color.white || cell.goThrough.Contains(GetCellObject(upRight)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, upRight);
                                texture.SetPixel(texCoordX + 1, texCoordY + 1, pixel);
                                banned.Add(new Vector2(texCoordX + 1, texCoordY + 1));
                            }
                            else if (left == Color.white || cell.goThrough.Contains(GetCellObject(left)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, left);
                                texture.SetPixel(texCoordX - 1, texCoordY, pixel);
                                banned.Add(new Vector2(texCoordX - 1, texCoordY));
                            }
                            else if (right == Color.white || cell.goThrough.Contains(GetCellObject(right)))
                            {
                                texture.SetPixel(texCoordX, texCoordY, right);
                                texture.SetPixel(texCoordX + 1, texCoordY, pixel);
                                banned.Add(new Vector2(texCoordX + 1, texCoordY));
                            }
                        }
                    }
                    else if (cell.useLiquidGravity)
                    {
                        if (down == Color.white || cell.goThrough.Contains(GetCellObject(down)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, down);
                            texture.SetPixel(texCoordX, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX, texCoordY - 1));
                        }
                        else if (downRight == Color.white || cell.goThrough.Contains(GetCellObject(downRight)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downRight);
                            texture.SetPixel(texCoordX + 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX + 1, texCoordY - 1));
                        }
                        else if (downLeft == Color.white || cell.goThrough.Contains(GetCellObject(downLeft)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, downLeft);
                            texture.SetPixel(texCoordX - 1, texCoordY - 1, pixel);
                            banned.Add(new Vector2(texCoordX - 1, texCoordY - 1));
                        }
                        else if (right == Color.white || cell.goThrough.Contains(GetCellObject(right)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, right);
                            texture.SetPixel(texCoordX + 1, texCoordY, pixel);
                            banned.Add(new Vector2(texCoordX + 1, texCoordY));
                        }
                        else if (left == Color.white || cell.goThrough.Contains(GetCellObject(left)))
                        {
                            texture.SetPixel(texCoordX, texCoordY, left);
                            texture.SetPixel(texCoordX - 1, texCoordY, pixel);
                            banned.Add(new Vector2(texCoordX - 1, texCoordY));
                        }
                    }
                    else if (cell.isStatic) { }
                    else
                    {
                        continue;
                    }
                    if (cell.corrode)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.corrodeSlowMod);
                        if (rnd == 1 && CellTypeDatabase.instance.solids.Contains(down) && CellTypeDatabase.instance.cellObjects[CellTypeDatabase.instance.solids.IndexOf(down)].destructible)
                        {
                            texture.SetPixel(texCoordX, texCoordY - 1, Color.white);
                        }
                    }
                    if (cell.morphOnCollision)
                    {
                        int rnd;

                        for (int i = 0; i < cell.morphCollision.Length; i++)
                        {
                            rnd = Random.Range(1, cell.morphSlowMod[i]);
                            if (rnd == 1 && (down == cell.morphCollision[i].color || up == cell.morphCollision[i].color || right == cell.morphCollision[i].color || left == cell.morphCollision[i].color))
                            {
                                texture.SetPixel(texCoordX, texCoordY, cell.morphInto[i].color);
                            }
                        }
                    }
                    if (cell.selfDecay)
                    {
                        int rnd;
                        rnd = Random.Range(1, cell.decaySlowMod);
                        if (rnd == 1)
                        {
                            if (cell.decayInto == null)
                            {
                                texture.SetPixel(texCoordX, texCoordY, Color.white);
                            }
                            else
                            {
                                texture.SetPixel(texCoordX, texCoordY, cell.decayInto.color);
                            }
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

    CellObject GetCellObject(Color color)
    {
        CellObject cell;

        if (CellTypeDatabase.instance.colors.Contains(color))
        {
            cell = CellTypeDatabase.instance.cellObjects[CellTypeDatabase.instance.colors.IndexOf(color)];
        }
        else
        {
            cell = null;
        }
        return cell;
    }
}