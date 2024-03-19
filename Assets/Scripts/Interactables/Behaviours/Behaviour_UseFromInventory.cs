using SnowHorse.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_UseFromInventory : MonoBehaviour, IBehaviour
{
    [SerializeField] private List<ItemData> requiredItems;
    [SerializeField] private List<GameObject> resultingBehaviours;
    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        RequireItems();
    }

    private void RequireItems()
    {
        for (int i = 0; i < requiredItems.Count; i++)
        {
            if (requiredItems[i] == Inventory.Instance.GetSelected)
            {
                Inventory.Instance.UseItem(requiredItems[i]);
                requiredItems.Remove(requiredItems[i]);
                break;
            }
        }

        if(requiredItems.Count <= 0 )
        {
            StartCoroutine(Result());
        }
    }

    private IEnumerator Result()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if(resultingBehaviours.Count <= 0)
        {
            WarningTool.Print("There are no resulting behaviours on:", gameObject);
        }
        else if (requiredItems.Count <= 0)
        {
            for(int i = 0; i < resultingBehaviours.Count;i++)
            {
                resultingBehaviours[i].GetComponent<IBehaviour>().Behaviour(true, true);
            }
        }
    }
}
