using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(Inventory.Instance.GetSelected != null)
            {
                Inventory.Instance.DeselectPreviousItem();
                Debug.Log("Deselecting inventory item from input controller!");
            }   
        }
    }
}
