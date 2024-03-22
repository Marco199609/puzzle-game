using UnityEngine;

[ExecuteInEditMode]
public class SaveableObject : MonoBehaviour
{
    public string Id;
    private void OnEnable()
    {
        if (string.IsNullOrEmpty(Id))
        {
            Id = System.Guid.NewGuid().ToString();
            Debug.Log($"Generated GUID for {gameObject}");
        }
    }
}
