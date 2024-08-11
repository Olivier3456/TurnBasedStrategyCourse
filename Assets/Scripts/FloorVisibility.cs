using System.Collections.Generic;
using UnityEngine;

public class FloorVisibility : MonoBehaviour
{
    [SerializeField] private bool dynamicFloorPosition = false;
    private Renderer[] rendererArray;
    [SerializeField] List<Renderer> ignoreRendererList;
    private int floor;


    private void Awake()
    {
        rendererArray = GetComponentsInChildren<Renderer>(true);
    }


    private void Start()
    {
        floor = GetFloor();

        if (floor == 0 && !dynamicFloorPosition)
        {
            Destroy(this);
        }
    }


    private void Update()
    {
        if (dynamicFloorPosition)
        {
            floor = GetFloor();
        }

        float cameraHeight = CameraController.Instance.GetCameraHeight();
        float floorHeightOffset = 2f;

        bool showObject = floor == 0 || cameraHeight > LevelGrid.FLOOR_HEIGHT * floor + floorHeightOffset;

        if (showObject)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }


    private int GetFloor()
    {
        return LevelGrid.Instance.GetFloor(transform.position);
    }


    private void Show()
    {
        foreach (Renderer renderer in rendererArray)
        {
            if (ignoreRendererList.Contains(renderer))
            {
                continue;
            }

            renderer.enabled = true;
        }
    }


    private void Hide()
    {
        foreach (Renderer renderer in rendererArray)
        {
            if (ignoreRendererList.Contains(renderer))
            {
                continue;
            }

            renderer.enabled = false;
        }
    }
}
