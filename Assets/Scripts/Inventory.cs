using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> inventory = new List<ItemData>();
    [SerializeField] private ItemData selectedItem;

    public static Inventory Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }

    public List<ItemData> GetInventoryItems { get => inventory; }
    public List<ItemData> Set { set => inventory = value; }

    public ItemData GetSelected { get => selectedItem; }

    public void Add(ItemData item)
    {
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

    public void UseItem(ItemData item)
    {
        if (inventory.Contains(item))
        {
            if (selectedItem == item) selectedItem = null;
            inventory.Remove(item);
            Destroy(item.gameObject);

            UIController.Instance.RemoveInventoryItem();
        }
        else Debug.Log($"Item {item.Name} does not exist in inventory!");
    }
}