using SnowHorse.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private float itemPreviewDuration = 1.0f;
    [SerializeField] private float goToSlotDuration = 1f;
    [SerializeField] private GameObject itemUIContainer;
    [SerializeField] private Image itemImagePrefab;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private List<Button> slots;

    private float currentLerpTime;

    public void ShowItem(ItemData item)
    {
        itemName.text = item.Name;
        var image = SetItemUISprite(item);

        for (int i = 0; i < slots.Count; i++)
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

        slot.gameObject.SetActive(true);
        itemUIContainer.SetActive(false);
        image.transform.SetParent(slot);

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

    private Image SetItemUISprite(ItemData item, bool preserveAspect = true)
    {
        var image = Instantiate(itemImagePrefab, itemImagePrefab.transform.position, Quaternion.identity, itemImagePrefab.transform.parent);
        image.sprite = item.Sprite;
        image.color = item.Color;
        image.preserveAspect = preserveAspect;

        return image;
    }
}