using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryUI))]
public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;

    [Header("Cursor")]
    [SerializeField] private Texture2D cross;
    [SerializeField] private Texture2D circle;

    private InventoryUI inventoryUI;

    public static UIController Instance;
    private void Awake()
    {
        if(Instance) Destroy(this);
        else Instance = this;

        inventoryUI = GetComponent<InventoryUI>();
    }

    public void ShowItem(ItemData item)
    {
        inventoryUI.ShowItem(item);
    }

    public void SetCursor(bool canInteract)
    {
        var cursor = canInteract ? circle : cross;
        Cursor.SetCursor(cursor, new Vector2(cursor.height / 2, cursor.width / 2), CursorMode.Auto);
    }
}