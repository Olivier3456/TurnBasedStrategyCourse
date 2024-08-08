using System.Collections.Generic;

public class GridObject
{
    private GridSystemHex<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitsList = new List<Unit>();
    private IInteractable interactable;

    public GridObject(GridSystemHex<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (var unit in unitsList)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        unitsList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitsList.Remove(unit);
    }

    public List<Unit> GetUnitslist()
    {
        return unitsList;
    }

    public bool HasAnyUnit()
    {
        return unitsList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitsList[0];
        }

        return null;
    }

    public IInteractable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
    }
}
