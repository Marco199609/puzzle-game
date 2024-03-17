using UnityEngine;

public class Behaviour_AddToInventory : MonoBehaviour, IBehaviour
{
    [SerializeField] private ItemData item;
    [SerializeField] private bool previewInUI;
    [SerializeField] private bool onInteraction;
    [SerializeField] private bool onInspection;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        if(Inventory.Instance.CanAddItems)
        {
            if (onInteraction && isInspecting || onInspection && isInspecting || onInteraction == isInteracting == onInspection == isInspecting == false) Inventory.Instance.Add(item);
        }
    }
}