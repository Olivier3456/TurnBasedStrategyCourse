using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;
    private GridPosition gridPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

    }

    private void Update()
    {
        unitAnimator.SetBool("isWalking", true);

        float currentDistance = Vector3.Distance(transform.position, targetPosition);
        float stoppingDistance = 0.5f;

        if (currentDistance > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

            unitAnimator.SetBool("isWalking", true);
        }
        else
        {
            unitAnimator.SetBool("isWalking", false);
        }


        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }

    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
