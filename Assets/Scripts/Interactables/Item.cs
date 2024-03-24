using System;
using UnityEngine;

[RequireComponent(typeof(ObjectGUID))]
public class Item : MonoBehaviour, IDataPersistence
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Vector2 ScaleInPreviewUI { get; private set; } = new Vector2(500, 500);
    [field: SerializeField] public Vector2 ScaleInInventoryUI { get; private set; } = new Vector2(120, 120);
    [field: SerializeField] public Vector2 RotationInPreviewUI { get; private set; }
    [field: SerializeField] public float ZRotationInInventory { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;

    [NonSerialized] public ObjectGUID Guid;
    [NonSerialized] public bool collected = false;
    [NonSerialized] public bool isInInventory = false;


    public void LoadData(GameData data)
    {
        Guid = GetComponent<ObjectGUID>();

        if (!Guid)
        {
            Debug.LogError($"No GUID on {gameObject}!");
        }
        else
        {
            data.ItemsInInventory.TryGetValue(Guid.Id, out isInInventory);
            data.ItemsCollected.TryGetValue(Guid.Id, out collected);

            if (isInInventory) Inventory.Instance.Add(item: this, previewOnUI: false, dataPersistenceMode: true);
            else if (collected) gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (!Guid)
        {
            Debug.LogError($"No saveable object on {gameObject}!");
        }
        else
        {
            if (data.ItemsCollected.ContainsKey(Guid.Id)) data.ItemsCollected.Remove(Guid.Id);
            data.ItemsCollected.Add(Guid.Id, collected);

            if (data.ItemsInInventory.ContainsKey(Guid.Id)) data.ItemsInInventory.Remove(Guid.Id);
            data.ItemsInInventory.Add(Guid.Id, isInInventory);
        }
    }
}
