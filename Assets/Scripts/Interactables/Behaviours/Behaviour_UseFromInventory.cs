using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Behaviour_UseFromInventory : MonoBehaviour, IBehaviour
{
    [SerializeField] private List<ItemData> requiredItems;
    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        for(int i = 0; i < requiredItems.Count; i++)
        {
            if (requiredItems[i] == Inventory.Instance.GetSelected)
            {
                Inventory.Instance.UseItem(requiredItems[i]);
                requiredItems.Remove(requiredItems[i]);
                break;
            }
        }
    }
}
