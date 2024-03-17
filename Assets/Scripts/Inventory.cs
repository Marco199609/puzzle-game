using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryUI))]
public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform usedItemsParent;

    private List<ItemData> inventory = new List<ItemData>();
    private ItemData selectedItem;
    private InventoryUI inventoryUI;

    public static Inventory Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        inventoryUI = GetComponent<InventoryUI>();
    }

    public List<ItemData> GetInventoryItems { get => inventory; }
    public ItemData GetSelected { get => selectedItem; }

    [NonSerialized] public bool CanAddItems = true;

    public void Add(ItemData item)
    {
        Deselect();
        inventoryUI.PreviewItem(item);
        item.gameObject.SetActive(false);
        item.gameObject.transform.SetParent(transform);
        inventory.Add(item);
    }

    public void Select(ItemData item)
    {
        if (inventory.Contains(item)) selectedItem = item;
        else Debug.Log($"Item {item.Name} does not exist in inventory!");
    }

    public void Deselect()
    {
        selectedItem = null;
        inventoryUI.DeselectSlot();
    }

    public void UseItem(ItemData item)
    {
        if (inventory.Contains(item))
        {
            if (selectedItem == item) selectedItem = null;
            inventory.Remove(item);
            item.transform.SetParent(usedItemsParent);

            StartCoroutine(inventoryUI.RemoveInventoryItem());
        }
        else Debug.Log($"Item {item.Name} does not exist in inventory!");
    }
}