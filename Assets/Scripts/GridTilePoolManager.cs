using System.Collections.Generic;
using UnityEngine;
public class GridTilePoolManager : MonoBehaviour
{
    [SerializeField] private GridTile tilePrefab;
    private Queue<GridTile> pool = new Queue<GridTile>();

    public GridTile GetTile()
    {
        if (pool.Count > 0)
        {
            GridTile tile = pool.Dequeue();
            tile.gameObject.SetActive(true);
            return tile;
        }
        else
        {
            return Instantiate(tilePrefab);
        }
    }

    public void ReturnTile(GridTile tile)
    {
        tile.gameObject.SetActive(false);
        pool.Enqueue(tile);
    }
}
