using SnowHorse.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private float itemPreviewDuration = 1.0f;
    [SerializeField] private float goToSlotDuration = 1f;
    [SerializeField] private GameObject itemUIContainer;
    [SerializeField] private Image itemImagePrefab;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Button[] slots;

    private float currentLerpTime;

    private void Start()
    {
        foreach (Button slot in slots)
        {
            slot.onClick.AddListener(RemoveInventoryItem);
        }
    }

    public void ShowItem(ItemData item)
    {
        itemName.text = item.Name;
        var image = SetItemUISprite(item);

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].gameObject.activeInHierarchy)
            {
                StartCoroutine(AddToInventoryUI(item, image, slots[i].transform));
                break;
            }
        }
    }

    private IEnumerator AddToInventoryUI(ItemData item, Image image, Transform slot)
    {
        itemUIContainer.SetActive(true);
        image.gameObject.SetActive(true);
        slot.gameObject.SetActive(true);
        image.transform.SetParent(slot);

        if (item.ScaleInPreviewUI != Vector2.zero) image.rectTransform.sizeDelta = item.ScaleInPreviewUI;
        else Debug.Log("Please set item UI preview scale!");

        if(item.ScaleInInventoryUI == Vector2.zero) Debug.Log("Please set item UI inventory scale!");

        var startPosition = image.transform.position;
        var targetPosition = slot.transform.position;

        var startScale = item.ScaleInPreviewUI;
        var targetScale = item.ScaleInInventoryUI;

        var startRotation = image.transform.rotation;
        var targetRotation = Quaternion.Euler(item.RotationInInventory);

        yield return new WaitForSeconds(itemPreviewDuration);

        itemUIContainer.SetActive(false);

        currentLerpTime = 0;
        float percentage = 0;

        while (percentage < 1)
        {
            percentage = Interpolation.Sinerp(goToSlotDuration, ref currentLerpTime);

            image.transform.position = Vector3.Lerp(startPosition, targetPosition, percentage);
            image.rectTransform.sizeDelta = Vector2.Lerp(startScale, targetScale, percentage);
            image.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percentage);

            yield return null;
        }

        Debug.Log("Finished moving inventory UI");
    }

    public void RemoveInventoryItem()
    {
        var slot = EventSystem.current.currentSelectedGameObject;

        var removed = false;

        if(slot.TryGetComponent(out Button button))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(!removed && button == slots[i])
                {
                    Inventory.Instance.Remove(Inventory.Instance.Get[i]);
                    Destroy(slot.transform.GetChild(0).gameObject);
                    removed = true;
                }
                
                if(removed)
                {
                    if (i < slots.Length - 1 && slots[i + 1].transform.childCount > 0)
                    {
                        slots[i + 1].transform.GetChild(0).SetParent(slots[i].transform);
                        slots[i].transform.GetChild(slots[i].transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition = Vector3.zero; //Remember Destroy previous child sets in at the end of frame
                    }
                    else
                    {
                        slots[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            Debug.Log($"Slot {slot.name} passed by the inventory to the UI is invalid!");
        }
    }

    private Image SetItemUISprite(ItemData item, bool preserveAspect = true)
    {
        var image = Instantiate(itemImagePrefab, itemImagePrefab.transform.position, Quaternion.identity, itemImagePrefab.transform.parent);
        image.sprite = item.Sprite;
        image.color = item.Color;
        image.preserveAspect = preserveAspect;

        return image;
    }
}