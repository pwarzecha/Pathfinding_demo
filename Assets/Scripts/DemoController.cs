using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemoController : MonoBehaviour
{
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private TextMeshProUGUI heightTextField;
    [SerializeField] private TextMeshProUGUI widthTextField;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ThirdPersonController thirdPersonController;

    private Pathfinder pathfinder;
    private List<GridTile> cachedPath;
    private GridTile startTile;
    private GridTile endTile;

    private void Start()
    {
        pathfinder = new Pathfinder(gridManager);
        thirdPersonController = Instantiate(thirdPersonController, Vector3.zero, Quaternion.identity);
        widthSlider.onValueChanged.AddListener(OnGridWidthChanged);
        heightSlider.onValueChanged.AddListener(OnGridHeightChanged);
        GridTile.OnTileStateChanged += OnTileStateChanged;
    }

    private void OnDestroy()
    {
        GridTile.OnTileStateChanged -= OnTileStateChanged;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleInput();
    }

    private void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out GridTile tile))
            {
                TileClicked(tile);
            }
        }
    }

    private void OnGridWidthChanged(float value)
    {
        gridManager.Width = (int)value;
        widthTextField.text = "Width: " + (int)value;
        CheckAndUpdatePath();
    }

    private void OnGridHeightChanged(float value)
    {
        gridManager.Height = (int)value;
        heightTextField.text = "Height: " + (int)value;
        CheckAndUpdatePath();
    }

    private void TileClicked(GridTile tile)
    {
        if (!tile.IsTraversable)
        {
            return;
        }

        if (startTile == null)
        {
            startTile = tile;
            tile.Highlight(true);
        }
        else if (endTile == null)
        {
            endTile = tile;
            tile.Highlight(true);
            CreatePath();
        }
        else
        {
            thirdPersonController.Stop();
            ClearCurrentPath();
            startTile.Highlight(false);
            startTile = null;
            endTile.Highlight(false);
            endTile = null;
        }
    }

    private void ClearCurrentPath()
    {
        if (cachedPath != null)
        {
            foreach (var p in cachedPath)
            {
                if (p != startTile && p != endTile)
                {
                    p.Highlight(false);
                }
            }
            cachedPath.Clear();
        }
    }

    private void CreatePath(bool isUpdate = false)
    {
        ClearCurrentPath();

        if (startTile != null && endTile != null)
        {
            cachedPath = pathfinder.CalculatePath(startTile.Coordinates, endTile.Coordinates);
            if (cachedPath.Count > 0)
            {
                List<Vector3> pathPositions = new List<Vector3>();
                foreach (var p in cachedPath)
                {
                    pathPositions.Add(p.transform.position + Vector3.up * 0.002f);
                    p.Highlight(true);
                }

                if (isUpdate)
                {
                    thirdPersonController.UpdatePath(pathPositions);
                }
                else
                {
                    thirdPersonController.SetPath(pathPositions);
                }
            }
            else
            {
                thirdPersonController.Stop();
            }
        }
        else
        {
            thirdPersonController.Stop();
        }
    }

    private void OnTileStateChanged(Vector2Int tilePos)
    {
        if (IsPathAffected(tilePos) || cachedPath.Count == 0)
        {
            CreatePath(true);
        }
    }

    private bool IsPathAffected(Vector2Int tilePos)
    {
        if (cachedPath != null)
        {
            foreach (GridTile tile in cachedPath)
            {
                if (tile.Coordinates == tilePos)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void CheckAndUpdatePath()
    {
        if (startTile != null && endTile != null)
        {
            CreatePath(true);
        }
    }
}
