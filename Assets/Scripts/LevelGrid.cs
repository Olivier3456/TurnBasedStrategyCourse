using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public const float FLOOR_HEIGHT = 3f;
    [SerializeField] private Transform gridDebugObjectPrefab;

    public event EventHandler OnAnyUnitMovedGridPosition;

    private List<GridSystem<GridObject>> gridSystemList;

    public static LevelGrid Instance { get; private set; }

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private int floorAmount;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("An instance of LevelGrid already exists!");
            Destroy(gameObject);
            return;
        }
        gridSystemList = new List<GridSystem<GridObject>>();

        for (int floor = 0; floor < floorAmount; floor++)
        {
            GridSystem<GridObject> gridSystem = new GridSystem<GridObject>(width, height, cellSize, floor, FLOOR_HEIGHT,
           (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
            gridSystemList.Add(gridSystem);
        }

        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }


    private GridSystem<GridObject> GetGridSystem(int floor)
    {
        return gridSystemList[floor];
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GetGridSystem(gridPosition.floor).GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitsListAtGridPosition(GridPosition gridPosition)
    {
        return GetGridSystem(gridPosition.floor).GetGridObject(gridPosition).GetUnitslist();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GetGridSystem(gridPosition.floor).GetGridObject(gridPosition).RemoveUnit(unit);
    }
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public int GetFloor(Vector3 worldPosition)
    {
        return Mathf.RoundToInt(worldPosition.y / FLOOR_HEIGHT);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        int floor = GetFloor(worldPosition);
        return GetGridSystem(floor).GetGridPosition(worldPosition);
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) => GetGridSystem(gridPosition.floor).GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => GetGridSystem(gridPosition.floor).IsValidGridPosition(gridPosition);

    public int GetWidth() => GetGridSystem(0).GetWidth();

    public int GetHeight() => GetGridSystem(0).getHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        return GetGridSystem(gridPosition.floor).GetGridObject(gridPosition).HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = GetGridSystem(gridPosition.floor).GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
}
