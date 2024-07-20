using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start()
    {
        UnitActionSystem.Instance.OnUnitSelectedChanged += UnitActionSystem_UnitSelectedChanged;
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_UnitSelectedChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        //Debug.Log("CreateUnitActionButtons");

        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }


        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }
}
