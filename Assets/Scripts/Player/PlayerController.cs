using UnityEngine;
using SnowHorse.Utils;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private bool canInteract;
    private Vector3 hitPoint;
    [SerializeField] private GameObject objectInSight;

    void Update()
    {
        Interact();
    }

    private void Interact()
    {
        var mousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if(mousePosition != Vector2.zero || Input.GetMouseButtonDown(0))
        {
            objectInSight = Raycaster.Cast2D(
                origin: _camera.ScreenToWorldPoint(Input.mousePosition),
                direction: _camera.transform.forward,
                maxDistance: 50,
                layerMask: -1,
                hitPoint: out hitPoint);

            if(objectInSight && objectInSight.TryGetComponent(out Interactable interactable))
            {
                canInteract = true;
                if(Input.GetMouseButtonDown(0)) interactable.Interact(true, true);
            }
            else
            {
                canInteract = false;
            }

            UIController.Instance.SetCursor(canInteract);
        }
    }
}