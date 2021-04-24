using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTypeDatabase : MonoBehaviour
{
    public static CellTypeDatabase instance;

    private void Awake()
    {
        instance = this;
    }

    public CellObject[] cellObjects;
    //[HideInInspector]
    public List<Color32> colors = new List<Color32>();
    //[HideInInspector]
    public List<Color32> solids = new List<Color32>();
    //[HideInInspector]
    public List<Color32> liquids = new List<Color32>();
    //[HideInInspector]
    public List<Color32> gasses = new List<Color32>();


    private void Start()
    {
        for (int i = 0; i < cellObjects.Length; i++)
        {
            colors.Add(cellObjects[i].color);
        }
        for (int i = 0; i < cellObjects.Length; i++)
        {
            if(cellObjects[i].state == StatesOfMatter.solid)
            {
                solids.Add(cellObjects[i].color);
            }
        }
        for (int i = 0; i < cellObjects.Length; i++)
        {
            if (cellObjects[i].state == StatesOfMatter.liquid)
            {
                liquids.Add(cellObjects[i].color);
            }
        }
        for (int i = 0; i < cellObjects.Length; i++)
        {
            if (cellObjects[i].state == StatesOfMatter.gas)
            {
                gasses.Add(cellObjects[i].color);
            }
        }
    }
}
