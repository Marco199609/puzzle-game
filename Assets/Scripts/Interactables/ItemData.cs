using UnityEngine;

public class ItemData : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Vector3 ScaleInUI { get; private set; }
    [field: SerializeField] public Vector3 ScaleInInventory { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
}