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
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);
                GridPosition gridPosition = new GridPosition(x, z);
                GridObject gridObject = new GridObject(this, gridPosition);
                gridObjectsArray[x, z] = gridObject;
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellsize;
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
               GameObject.Instantiate(debugPrefab, GetWorldPosition(x, z), Quaternion.identity);
            }
        }
    }
}
