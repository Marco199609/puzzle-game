using UnityEngine;

public class ItemData : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Vector2 ScaleInPreviewUI { get; private set; } = new Vector2(500, 500);
    [field: SerializeField] public Vector2 ScaleInInventoryUI { get; private set; } = new Vector2(150, 150);
    [field: SerializeField] public Vector2 RotationInPreviewUI { get; private set; }
    [field: SerializeField] public Vector3 RotationInInventory { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
}