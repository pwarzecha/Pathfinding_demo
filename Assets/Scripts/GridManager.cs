using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridTilePoolManager gridTilePoolManager;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;

    private List<List<GridTile>> grid = new List<List<GridTile>>();

    public int Width { get => width; set => SetWidth(value); }
    public int Height { get => height; set => SetHeight(value); }

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int x = 0; x < width; x++)
        {
            List<GridTile> row = new List<GridTile>();
            for (int z = 0; z < height; z++)
            {
                GridTile tile = gridTilePoolManager.GetTile();
                tile.transform.position = new Vector3(x, 0, z);
                tile.transform.parent = this.transform;
                tile.Init(new Vector2Int(x, z), true);
                row.Add(tile);
            }
            grid.Add(row);
        }
    }

    private void SetWidth(int newWidth)
    {
        if (newWidth > width)
        {
            for (int x = width; x < newWidth; x++)
            {
                List<GridTile> newRow = new List<GridTile>();
                for (int z = 0; z < height; z++)
                {
                    GridTile tile = gridTilePoolManager.GetTile();
                    tile.transform.position = new Vector3(x, 0, z);
                    tile.transform.parent = this.transform;
                    tile.Init(new Vector2Int(x, z), true);
                    newRow.Add(tile);
                }
                grid.Add(newRow);
            }
        }
        else if (newWidth < width)
        {
            for (int x = width - 1; x >= newWidth; x--)
            {
                List<GridTile> row = grid[x];
                foreach (var tile in row)
                {
                    gridTilePoolManager.ReturnTile(tile);
                }
                grid.RemoveAt(x);
            }
        }
        width = newWidth;
    }

    private void SetHeight(int newHeight)
    {
        if (newHeight > height)
        {
            for (int x = 0; x < width; x++)
            {
                List<GridTile> row = grid[x];
                for (int z = height; z < newHeight; z++)
                {
                    GridTile tile = gridTilePoolManager.GetTile();
                    tile.transform.position = new Vector3(x, 0, z);
                    tile.transform.parent = this.transform;
                    tile.Init(new Vector2Int(x, z), true);
                    row.Add(tile);
                }
            }
        }
        else if (newHeight < height)
        {
            for (int x = 0; x < width; x++)
            {
                List<GridTile> row = grid[x];
                for (int z = height - 1; z >= newHeight; z--)
                {
                    gridTilePoolManager.ReturnTile(row[z]);
                    row.RemoveAt(z);
                }
            }
        }
        height = newHeight;
    }

    public GridTile GetTileAt(Vector2Int coordinates)
    {
        if (coordinates.x >= 0 && coordinates.x < width && coordinates.y >= 0 && coordinates.y < height)
        {
            return grid[coordinates.x][coordinates.y];
        }
        return null;
    }

    public List<GridTile> GetNeighbors(GridTile tile)
    {
        if (tile == null)
            return new List<GridTile>(); 
        List<GridTile> neighbors = new List<GridTile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x != 0 && z != 0)
                    continue;

                int checkX = tile.Coordinates.x + x;
                int checkZ = tile.Coordinates.y + z;

                if (checkX >= 0 && checkX < width && checkZ >= 0 && checkZ < height)
                {
                    neighbors.Add(grid[checkX][checkZ]);
                }
            }
        }
        return neighbors;
    }
}
