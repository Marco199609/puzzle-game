using UnityEngine;

public class Behaviour_AddToInventory : MonoBehaviour, IBehaviour
{
    [SerializeField] private ItemData item;
    [SerializeField] private bool previewInUI;
    [SerializeField] private bool onInteraction;
    [SerializeField] private bool onInspection;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        UIController.Instance.ShowItem(item);
        item.gameObject.SetActive(false);
    }
}