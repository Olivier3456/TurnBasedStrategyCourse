using System.Collections.Generic;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;

    private List<Unit> unitsList = new List<Unit>();

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
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
}
