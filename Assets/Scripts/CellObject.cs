using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

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
    [Tooltip("The name of this particle.")]
    public string cellName;

    [Tooltip("The state of matter of this particle.")]
    public StatesOfMatter state;

    [HideInInspector]
    public bool isSolid;
    [HideInInspector]
    public bool isLiquid;
    [HideInInspector]
    public bool isGas;

    [Header("GravityType")]

    [Tooltip("Enable this to make the particle static and unaffected by any form of force.")]
    public bool isStatic = false;

    [EnableIf("isSolid")] [HideIf("isStatic")] [Tooltip("Gravity that makes the particle fall into tower-shaped piles.")]
    public bool useRigidGravity = false;
    [EnableIf("isSolid")] [HideIf("isStatic")] [Tooltip("Gravity that makes the particle fall into pyramid-shaped piles.")]
    public bool usePowderGravity = false;
    [EnableIf("isLiquid")] [HideIf("isStatic")] [Tooltip("Gravity that makes the particle fall and spread to fill the available space.")]
    public bool useLiquidGravity = false;
    [EnableIf("isGas")] [HideIf("isStatic")] [Tooltip("Gravity that makes the particle rise into pyramid-shaped piles.")]
    public bool useGasGravity = false;

    [Header("Properties")]

    [Tooltip("Enable this to make this particle corrosive.")]
    public bool corrode = false;
    [ShowIf("corrode")] [Tooltip("The chance every frame for this particle to corrode the particle beneath it.")]
    public int corrodeSlowMod = 7;

    [Tooltip("Enable this to make this particle decay after some time into another.")]
    public bool selfDecay = false;
    [ShowIf("selfDecay")] [Tooltip("The chance every frame for this particle to decay.")]
    public int decaySlowMod;
    [ShowIf("selfDecay")] [Tooltip("The particle this particle should decay into (leave empty for it to be destroyed)")]
    public CellObject decayInto;

    [Tooltip("Enable this to make this particle morph into another after colliding with the particle of your choice.")]
    public bool morphOnCollision;
    [ShowIf("morphOnCollision")]
    public int numInteractions;
    [ShowIf("morphOnCollision")] [Tooltip("The chance (after colliding) every frame for this particle to morph.")]
    public int[] morphSlowMod;
    [ShowIf("morphOnCollision")] [Tooltip("The particle that, when this particle collides with it, this particle will morph.")]
    public CellObject[] morphCollision;
    [ShowIf("morphOnCollision")] [Tooltip("The particle that this particle will morph into after colliding with the morph collision particle.")]
    public CellObject[] morphInto;

    public List<CellObject> goThrough = new List<CellObject>();

    [Tooltip("Enable this to make your particle modifiable by other particles.")]
    public bool destructible;

    [ShowIf("useGasGravity")] [Tooltip("The chance every frame for your particle to rise.")]
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

        Array.Resize(ref morphSlowMod, numInteractions);
        Array.Resize(ref morphCollision, numInteractions);
        Array.Resize(ref morphInto, numInteractions);
    }
}
