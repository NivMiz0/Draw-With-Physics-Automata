using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCellPalettte : MonoBehaviour
{
    public PlaceCell placeCell;
    public Animator animator;
    public Texture2D texture;

    public Animator nameAnim;
    public Text nameText;

    public void SwapCell(CellObject cell)
    {
        placeCell.selectedCell = cell;
        nameText.text = cell.cellName;
        nameAnim.SetTrigger("FadeIn");
    }

    public void ClearBoard()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (x == texture.width - 1 || x == 0 || y == texture.height - 1)
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


    public void EnterPalette()
    {
        animator.SetTrigger("Enter");
    }

    public void ExitPalette()
    {
        animator.SetTrigger("Exit");
    }
}
