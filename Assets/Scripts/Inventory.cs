using System.Collections.Generic;
using UnityEngine;

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

    public List<ItemData> Get { get => inventory; }
    public List<ItemData> Set { set => inventory = value; }

    public ItemData GetSelected { get => selectedItem; }

    public void Add(ItemData item)
    {
        UIController.Instance.ShowItem(item);
        item.gameObject.SetActive(false);
        item.gameObject.transform.SetParent(transform);
        inventory.Add(item);
    }

    public void Select(ItemData item)
    {
        if (inventory.Contains(item)) selectedItem = item;
        else Debug.Log($"Item {item.Name} does not exist in inventory!");
    }

    public void Use(ItemData item)
    {
        selectedItem = null;
        inventory.Remove(item);
        //TODO: Update UI
    }
}