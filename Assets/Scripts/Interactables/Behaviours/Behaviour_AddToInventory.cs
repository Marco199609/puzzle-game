using UnityEngine;

public class Behaviour_AddToInventory : MonoBehaviour, IBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private bool previewInUI = true;
    [SerializeField] private bool onInteraction;
    [SerializeField] private bool onInspection;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        if(Inventory.Instance.CanAddItems)
        {
            if (onInteraction && isInspecting || onInspection && isInspecting || onInteraction == isInteracting == onInspection == isInspecting == false)
            {
            	item.collected = true;
            	Inventory.Instance.Add(item, previewInUI);
            }
        }
    }
}