using SnowHorse.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Behaviour_UseFromInventory : MonoBehaviour, IBehaviour, IDataPersistence
{
    [SerializeField] private List<Item> requiredItems;
    [SerializeField] private List<GameObject> resultingBehaviours;

    [SerializeField] private ObjectGUID guid;

    private List<string> usedItemIds = new List<string>();
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
                usedItemIds.Add(requiredItems[i].Guid.Id);
                requiredItems.Remove(requiredItems[i]);
                break;
            }
        }

        if(requiredItems.Count <= 0 ) StartCoroutine(Result());
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

    public void LoadData(GameData data)
    {
        if (!guid)
        {
            Debug.LogError($"No saveable object on {gameObject}!");
        }
        else
        {
            usedItemIds = new List<string>();

            if (data.RequiredItemsUsed.ContainsKey(guid.Id))
            {
                var requiredItemsUsed = data.RequiredItemsUsed[guid.Id];

                foreach (Item item in requiredItems.ToList())
                {
                    if (requiredItemsUsed.Contains(item.Guid.Id))
                    {
                        usedItemIds.Add(item.Guid.Id);
                        requiredItems.Remove(item);
                    }
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.RequiredItemsUsed.ContainsKey(guid.Id)) data.RequiredItemsUsed.Remove(guid.Id);
        data.RequiredItemsUsed.Add(guid.Id, usedItemIds);
    }
}