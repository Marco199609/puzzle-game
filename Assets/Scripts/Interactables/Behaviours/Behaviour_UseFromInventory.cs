using SnowHorse.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Behaviour_UseFromInventory : MonoBehaviour, IBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [SerializeField] private List<Item> requiredItems;
    [SerializeField] private List<GameObject> resultingBehaviours;

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
                usedItemIds.Add(requiredItems[i].id);
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

    public void LoadData(GameData data)
    {
        usedItemIds = new List<string>();

        if(data.RequiredItemsUsed.ContainsKey(id))
        {
            var requiredItemsUsed = data.RequiredItemsUsed[id];

            foreach(Item item in requiredItems.ToList())
            {
                if (requiredItemsUsed.Contains(item.id))
                {
                    usedItemIds.Add(item.id);
                    requiredItems.Remove(item);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.RequiredItemsUsed.ContainsKey(id))
        {
            data.RequiredItemsUsed.Remove(id);
        }

        data.RequiredItemsUsed.Add(id, usedItemIds);
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
}
