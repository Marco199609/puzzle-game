using UnityEngine;

public class Behaviour_AddToInventory : MonoBehaviour, IBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private bool previewInUI = true;
    [SerializeField] private bool onInteraction = true;
    [SerializeField] private bool onInspection;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        if(!item) GetItem();

        if(item && Inventory.Instance.CanAddItems)
        {
            if (onInteraction && isInspecting || onInspection && isInspecting || onInteraction == isInteracting == onInspection == isInspecting == false)
            {
            	item.collected = true;
            	Inventory.Instance.Add(item, previewInUI);
            }
        }
    }

    private void GetItem()
    {
        Debug.LogWarning($"Retrieving inventory item automatically on game object: ({gameObject.name}), parent: ({gameObject.transform.parent.name})! Check if this is desired!");

        if (transform.parent.TryGetComponent(out Item item)) this.item = item;
        else if (transform.TryGetComponent(out Item thisItem)) this.item = thisItem;
        else Debug.LogError($"Could not find inventory item script on game object: ({gameObject.name}), parent: ({gameObject.transform.parent.name})!");
    }
}