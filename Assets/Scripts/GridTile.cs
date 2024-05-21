using System;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material traversableMaterial;
    [SerializeField] private Material obstacleMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Vector2Int coordinates;
    [SerializeField] private bool isTraversable;
    private GridTile gridParent;
    private int gCost;
    private int hCost;

    public Vector2Int Coordinates => coordinates;
    public bool IsTraversable => isTraversable;
    public GridTile GridParent { get => gridParent; set => gridParent = value; }
    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public int FCost => GCost + HCost;
    public static Action<Vector2Int> OnTileStateChanged;

    public void Init(Vector2Int coords, bool isTraversable)
    {
        this.isTraversable = isTraversable;
        coordinates = coords;
        meshRenderer.material = isTraversable ? traversableMaterial : obstacleMaterial;
    }


    public void ToggleState()
    {
        isTraversable = !isTraversable;
        meshRenderer.material = isTraversable ? traversableMaterial : obstacleMaterial;
        OnTileStateChanged?.Invoke(coordinates);
        //Debug.LogError("toggled tile: " + coordinates);
    }

    public void Highlight(bool enable)
    {
        meshRenderer.material = enable ? highlightMaterial : isTraversable ? traversableMaterial : obstacleMaterial;
    }
}