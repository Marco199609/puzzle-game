using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryUI))]
public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform usedItemsParent;

    private int maxInventoryCount;
    private List<Item> inventory = new List<Item>();
    private Item selectedItem;
    private InventoryUI inventoryUI;

    public static Inventory Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        inventoryUI = GetComponent<InventoryUI>();
        maxInventoryCount = inventoryUI.GetSlots.Length;
    }

    public List<Item> GetInventoryItems { get => inventory; }
    public Item GetSelected { get => selectedItem; }

    [NonSerialized] public bool CanAddItems = true;

    public void Add(Item item, bool previewOnUI, bool dataPersistenceMode = false)
    {
        if(inventory.Count < maxInventoryCount)
        {
            DeselectPreviousItem();
            inventoryUI.Add(item, previewOnUI, dataPersistenceMode);
            item.gameObject.SetActive(false);
            item.gameObject.transform.SetParent(transform);
            inventory.Add(item);
            item.isInInventory = true;
        }
        else
        {
            Debug.Log("Inventory full!");
        }

    }

    public void Select(Item item)
    {
        if (inventory.Contains(item)) selectedItem = item;
        else Debug.Log($"Item {item.Name} does not exist in inventory!");
    }

    public void DeselectPreviousItem()
    {
        selectedItem = null;
        inventoryUI.DeselectSlot();
    }

    public void UseItem(Item item)
    {
        if (inventory.Contains(item))
        {
            if (selectedItem == item) selectedItem = null;
            inventory.Remove(item);
            item.isInInventory = false;
            item.transform.SetParent(usedItemsParent);

            StartCoroutine(inventoryUI.RemoveInventoryItem());
        }
        else
        {
            Debug.Log($"Item {item.Name} does not exist in inventory!");
        }
    }
}