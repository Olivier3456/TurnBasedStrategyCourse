using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private static MouseWorld instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, MouseWorld.instance.mousePlaneLayerMask);
        return raycastHit.point;
    }

    public static Vector3 GetPosition_OnlyHitVisible()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());

        RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, float.MaxValue, MouseWorld.instance.mousePlaneLayerMask);

        // Sort the raycast hit array by distance.
        System.Array.Sort(raycastHitArray, (RaycastHit a, RaycastHit b) =>
        {
            return Mathf.RoundToInt(a.distance - b.distance);
        });

        foreach (RaycastHit raycastHit in raycastHitArray)
        {
            if (raycastHit.transform.TryGetComponent(out Renderer renderer))
            {
                if (renderer.enabled)
                {
                    return raycastHit.point;    // Return the nearest hit with a collider enabled.
                }
            }
        }

        return Vector3.zero;
    }
}
