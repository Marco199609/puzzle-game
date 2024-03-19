using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    [field: SerializeField] public string id;
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Vector2 ScaleInPreviewUI { get; private set; } = new Vector2(500, 500);
    [field: SerializeField] public Vector2 ScaleInInventoryUI { get; private set; } = new Vector2(120, 120);
    [field: SerializeField] public Vector2 RotationInPreviewUI { get; private set; }
    [field: SerializeField] public Vector3 RotationInInventory { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }

    public bool collected = false;
    public bool IsInInventory = false;

    public void LoadData(GameData data)
    {
        data.ItemsCollected.TryGetValue(id, out collected);
        if(collected) gameObject.SetActive(false);

        data.InventoryItems.TryGetValue(id, out IsInInventory);
        if (IsInInventory) Inventory.Instance.Add(item: this, previewOnUI: false, dataPersistenceMode: true);
    }

    public void SaveData(ref GameData data)
    {
        if(data.ItemsCollected.ContainsKey(id)) data.ItemsCollected.Remove(id);
        data.ItemsCollected.Add(id, collected);

        if(data.InventoryItems.ContainsKey(id)) data.InventoryItems.Remove(id);
        data.InventoryItems.Add(id, IsInInventory);
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
}
