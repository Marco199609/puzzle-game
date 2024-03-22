using System;
using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Vector2 ScaleInPreviewUI { get; private set; } = new Vector2(500, 500);
    [field: SerializeField] public Vector2 ScaleInInventoryUI { get; private set; } = new Vector2(120, 120);
    [field: SerializeField] public Vector2 RotationInPreviewUI { get; private set; }
    [field: SerializeField] public float ZRotationInInventory { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;

    [field: SerializeField] public SaveableObject SaveableObject { get; private set; }

    [NonSerialized] public bool collected = false;
    [NonSerialized] public bool isInInventory = false;


    public void LoadData(GameData data)
    {
        if(!SaveableObject)
        {
            Debug.LogError($"No saveable object on {gameObject}!");
        }
        else
        {
            data.ItemsInInventory.TryGetValue(SaveableObject.Id, out isInInventory);
            data.ItemsCollected.TryGetValue(SaveableObject.Id, out collected);

            if (isInInventory) Inventory.Instance.Add(item: this, previewOnUI: false, dataPersistenceMode: true);
            else if (collected) gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (!SaveableObject)
        {
            Debug.LogError($"No saveable object on {gameObject}!");
        }
        else
        {
            if (data.ItemsCollected.ContainsKey(SaveableObject.Id)) data.ItemsCollected.Remove(SaveableObject.Id);
            data.ItemsCollected.Add(SaveableObject.Id, collected);

            if (data.ItemsInInventory.ContainsKey(SaveableObject.Id)) data.ItemsInInventory.Remove(SaveableObject.Id);
            data.ItemsInInventory.Add(SaveableObject.Id, isInInventory);
        }
    }
}
