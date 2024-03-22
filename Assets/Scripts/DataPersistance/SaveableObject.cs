using UnityEngine;

[ExecuteInEditMode]
public class SaveableObject : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }
    private void OnEnable()
    {
        if (string.IsNullOrEmpty(Id))
        {
            Id = System.Guid.NewGuid().ToString();
            Debug.Log($"Generated GUID for {gameObject}.");
        }
    }
}
