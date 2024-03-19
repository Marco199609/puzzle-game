using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(Inventory.Instance.GetSelected != null) Inventory.Instance.DeselectPreviousItem();
        }
    }
}
