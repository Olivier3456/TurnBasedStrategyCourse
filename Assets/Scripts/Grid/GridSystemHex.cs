using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemHex<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = 0.75f;
    private int width;
    private int height;
    private float cellsize;
    private TGridObject[,] gridObjectsArray;

    public GridSystemHex(int width,
                        int height,
                        float cellsize,
                        Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;

        gridObjectsArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                TGridObject gridObject = createGridObject(this, gridPosition);
                gridObjectsArray[x, z] = gridObject;
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        Vector3 XOffsetForOddRaws = Vector3.right * cellsize * 0.5f;

        return new Vector3(gridPosition.x, 0, 0) * cellsize +
                ((gridPosition.z % 2 == 0) ? Vector3.zero : XOffsetForOddRaws) +
                new Vector3(0, 0, gridPosition.z) * cellsize * HEX_VERTICAL_OFFSET_MULTIPLIER;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellsize),
            Mathf.RoundToInt(worldPosition.z / cellsize)
            );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform deBugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = deBugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectsArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
            gridPosition.z >= 0 &&
            gridPosition.x < width &&
            gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }
}
