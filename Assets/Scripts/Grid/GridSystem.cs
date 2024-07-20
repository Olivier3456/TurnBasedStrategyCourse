using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellsize;

    private GridObject[,] gridObjectsArray = new GridObject[1, 1];

    public GridSystem(int width, int height, float cellsize)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;

        gridObjectsArray = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GridObject gridObject = new GridObject(this, gridPosition);
                gridObjectsArray[x, z] = gridObject;
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellsize;
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

    public GridObject GetGridObject(GridPosition gridPosition)
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
