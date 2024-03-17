using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform usedItemsParent;
    [SerializeField] private List<ItemData> inventory = new List<ItemData>();
    [SerializeField] private ItemData selectedItem;

    public static Inventory Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }

    public List<ItemData> GetInventoryItems { get => inventory; }
    public ItemData GetSelected { get => selectedItem; }

    [NonSerialized] public bool CanAddItems = true;

    public void Add(ItemData item)
    {
        Deselect();
        UIController.Instance.ShowInventoryItem(item);
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
        UIController.Instance.DeselectSlot();
    }

    public void UseItem(ItemData item)
    {
        if (inventory.Contains(item))
        {
            if (selectedItem == item) selectedItem = null;
            inventory.Remove(item);
            item.transform.SetParent(usedItemsParent);

            UIController.Instance.RemoveInventoryItem();
        }
        else Debug.Log($"Item {item.Name} does not exist in inventory!");
    }
}