using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen;
    private bool isActive;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractComplete;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isActive = false;
            onInteractComplete();
        }
    }



    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 1f;

        Debug.Log("Door.Interact()");

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
