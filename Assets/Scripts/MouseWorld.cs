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
}
