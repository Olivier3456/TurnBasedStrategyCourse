using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField] private Unit unit;
    [SerializeField] private GridSystemVisual gridSystemVisual;

    //void Start()
    //{


    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            unit.GetMoveAction().GetValidActionGridPositionList();

            GridSystemVisual.Instance.HideAllGridPosition();
            GridSystemVisual.Instance.ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
        }
    }
}
