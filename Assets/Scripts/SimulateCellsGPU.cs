using System.Collections;
using UnityEngine;


public struct CellData
{
    public Vector2 position;
    Color color;
};

public struct CellRelations
{
    public CellData self;
    public CellData down;
    public CellData downRight;
    public CellData downLeft;
    public CellData right;
    public CellData left;
    public CellData up;
    public CellData upRight;
    public CellData upLeft;
};

public class SimulateCellsGPU : MonoBehaviour
{
    [SerializeField] private Vector2Int simulationSize;
    [SerializeField] private ComputeShader computeShader;

    private Renderer myRenderer;
    [SerializeField] private RenderTexture renderTexture;
    private int mainKernel;
    private int initKernel;
    public float nextEpochInterval = 0.04f;

    CellRelations[] data;
    ComputeBuffer buffer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        Init(true);
    }

    public void Init(bool createNewTextures)
    {
        if (createNewTextures)
        {
            renderTexture = GetNewRenderTexture();
            Resources.UnloadUnusedAssets();
        }
        myRenderer.material.SetTexture("_MainTex", renderTexture);

        mainKernel = computeShader.FindKernel("CSMain");
        initKernel = computeShader.FindKernel("CSInit");
        computeShader.SetTexture(initKernel, "Result", renderTexture);
        computeShader.Dispatch(initKernel, (simulationSize.x / 32) + 1, (simulationSize.y / 32) + 1, 1);
        data = new CellRelations[simulationSize.x * simulationSize.y];
        int posSize = sizeof(float) * 2;
        int colorSize = sizeof(float) * 4;
        int total = posSize + colorSize;
        buffer = new ComputeBuffer(data.Length, 2160);
        buffer.SetData(data);

        computeShader.SetBuffer(0, "cells", buffer);
    }

    private RenderTexture GetNewRenderTexture()
    {
        var renderTexture = new RenderTexture(simulationSize.x, simulationSize.y, 24, RenderTextureFormat.ARGB32)
        {
            filterMode = FilterMode.Point,
            enableRandomWrite = true
        };
        renderTexture.Create();
        return renderTexture;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(nextEpochInterval);
            //computeShader.Dispatch(mainKernel, (simulationSize.x / 32) + 1, (simulationSize.y / 32) + 1, 1);
        }
    }

    private void OnDestroy()
    {
        renderTexture.Release();
    }

    // Palette for chaging values and stuff;

    /*public void ChangeEpochInterval(float value)
    {
        nextEpochInterval = value * value;
    }

    public void SetNewSize(Vector2Int newSize)
    {
        simulationSize = newSize;
        InitSimulation(true);
    }*/
}