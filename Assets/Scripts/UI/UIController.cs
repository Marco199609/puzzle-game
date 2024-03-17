using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;

    [Header("Cursor")]
    [SerializeField] private Texture2D cross;
    [SerializeField] private Texture2D circle;
    [SerializeField] private InventoryUI inventoryUI;

    public static UIController Instance;

    private void Awake()
    {
        if(Instance) Destroy(this);
        else Instance = this;
    }

    #region Cursor
    public void SetCursor(bool canInteract)
    {
        var cursor = canInteract ? circle : cross;
        Cursor.SetCursor(cursor, new Vector2(cursor.height / 2, cursor.width / 2), CursorMode.Auto);
    }
    #endregion

    #region Inventory
    public void ShowInventoryItem(ItemData item)
    {
        inventoryUI.PreviewItem(item);
    }

    public void RemoveInventoryItem()
    {
        StartCoroutine(inventoryUI.RemoveInventoryItem());
    }

    public void DeselectSlot()
    {
        inventoryUI.DeselectSlot();
    }
    #endregion
}