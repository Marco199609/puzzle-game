using UnityEngine;

[RequireComponent (typeof(Raycaster))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Sprite cross;
    [SerializeField] private Sprite circle;
    [SerializeField] private SpriteRenderer cursor;

    private bool canInteract;
    private Vector3 hitPoint;
    [SerializeField] private GameObject objectInSight;
    private Raycaster raycaster;

    private void Awake()
    {
        Cursor.visible = false;
        raycaster = GetComponent<Raycaster>();
    }

    void Update()
    {
        Interact();
    }

    private void Interact()
    {
        var mousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if(mousePosition != Vector2.zero || Input.GetMouseButtonDown(0))
        {
            objectInSight = raycaster.Cast2D(
                origin: _camera.ScreenToWorldPoint(Input.mousePosition),
                direction: _camera.transform.forward,
                maxDistance: 50,
                layerMask: -1,
                hitPoint: out hitPoint,
                debugRay: true,
                debugColor: Color.red);

            if(objectInSight && objectInSight.TryGetComponent(out Interactable interactable))
            {
                canInteract = true;
                if(Input.GetMouseButtonDown(0)) interactable.Interact(true, true);
            }
            else
            {
                canInteract = false;
            }

            CursorControl(canInteract);
        }
    }

    private void CursorControl(bool canInteract)
    {
        cursor.sprite = canInteract ? circle : cross;
        var cursorPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        cursor.gameObject.transform.position = new Vector3(cursorPosition.x, cursorPosition.y, -1);
    }
}