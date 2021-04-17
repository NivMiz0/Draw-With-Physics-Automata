using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum StatesOfMatter
{
    solid,
    liquid,
    gas,
    plasma,
    boseEinsteinCondensate
}
[CreateAssetMenu(fileName = "New Cell")]
public class CellObject : ScriptableObject
{
    public Color32 color;

    [Header("Info")]
    public string cellName;
    public string cellDescrip;

    [Header("State")]

    public StatesOfMatter state;

    [HideInInspector]
    public bool isSolid;
    [HideInInspector]
    public bool isLiquid;
    [HideInInspector]
    public bool isGas;

    [Header("GravityType")]

    [EnableIf("isSolid")]
    public bool useRigidGravity = false;
    [EnableIf("isSolid")]
    public bool usePowderGravity = false;
    [EnableIf("isLiquid")]
    public bool useLiquidGravity = false;
    [EnableIf("isGas")]
    public bool useGasPowderGravity = false;

    [Header("Properties")]
    public bool corrode = false;

    [ShowIf("corrode")]
    public int corrodeSlowMod = 7;


    public bool harden = false;

    [ShowIf("harden")]
    public CellObject hardenInto;

    [ShowIf("harden")]
    public int hardenSlowMod = 7;

    public bool destructible;

    [ShowIf("useGasPowderGravity")]
    public int floatSlowMod = 10;

    private void OnValidate()
    {
        if(state == StatesOfMatter.solid)
        {
            isSolid = true;
        }
        else
        {
            isSolid = false;
        }
        if (state == StatesOfMatter.liquid)
        {
            isLiquid = true;
        }
        else
        {
            isLiquid = false;
        }
        if (state == StatesOfMatter.gas)
        {
            isGas = true;
        }
        else
        {
            isGas = false;
        }
    }
}
