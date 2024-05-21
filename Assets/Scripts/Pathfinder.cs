using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private GridManager gridManager;

    public Pathfinder(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<GridTile> CalculatePath(Vector2Int startPos, Vector2Int targetPos)
    {
        GridTile startTile = gridManager.GetTileAt(startPos);
        GridTile targetTile = gridManager.GetTileAt(targetPos);

        List<GridTile> openSet = new List<GridTile>();
        HashSet<GridTile> closedSet = new HashSet<GridTile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            if (startTile == null || targetTile == null)
                return new List<GridTile>();

            GridTile currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].HCost < currentTile.HCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            foreach (GridTile neighbor in gridManager.GetNeighbors(currentTile))
            {
                if (!neighbor.IsTraversable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentTile.GCost + GetDistance(currentTile, neighbor);
                if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetTile);
                    neighbor.GridParent = currentTile;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<GridTile>();
    }

    private List<GridTile> RetracePath(GridTile startTile, GridTile endTile)
    {
        List<GridTile> path = new List<GridTile>();
        GridTile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.GridParent;
        }
        path.Add(startTile);
        path.Reverse();
        return path;
    }

    private int GetDistance(GridTile tileA, GridTile tileB)
    {
        int distX = Mathf.Abs(tileA.Coordinates.x - tileB.Coordinates.x);
        int distZ = Mathf.Abs(tileA.Coordinates.y - tileB.Coordinates.y);
        return distX + distZ;
    }
}
