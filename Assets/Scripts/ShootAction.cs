using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private float totalSpinAmount;

    private int maxShootDistance = 7;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;

        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
    }




    public override string getActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))  // Not on the grid.
                {
                    continue;
                }


                int testDistance = Mathf.Abs(x) + Mathf.Abs(z); // We want only a positive value.
                if (testDistance > maxShootDistance)
                {
                    continue;
                }


                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))  // No unit on this grid position.
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy()) // The target is on the same team as the unit.
                {
                    continue;
                }


                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }
}
