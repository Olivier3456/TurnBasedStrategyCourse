using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem gridSystem;

    public static LevelGrid Instance { get; private set; }


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

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }


    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitsListAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitslist();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

}
