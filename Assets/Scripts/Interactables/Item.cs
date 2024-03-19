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

    public bool Collected = false;

    public void LoadData(GameData data)
    {
        data.itemsCollected.TryGetValue(id, out Collected);

        if(Collected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if(data.itemsCollected.ContainsKey(id))
        {
            data.itemsCollected.Remove(id);
        }

        data.itemsCollected.Add(id, Collected);
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
}
