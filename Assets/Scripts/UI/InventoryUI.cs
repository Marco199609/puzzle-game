using SnowHorse.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private float goToPreviewDuration = 0.5f;
    [SerializeField] private float itemPreviewDuration = 0.3f;
    [SerializeField] private float goToSlotDuration = 0.7f;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject itemUIContainer;
    [SerializeField] private Image itemImagePrefab;
    [SerializeField] private Button[] slots;

    private Button selectedSlot;
    private List<Coroutine> movingItems = new List<Coroutine>();

    private void Start()
    {
        foreach(var slot in slots)
        {
            slot.onClick.AddListener(() => { SelectSlot(); });
        }
    }

    private void SelectSlot()
    {
        selectedSlot = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (selectedSlot.gameObject == slots[i].gameObject)
            {
                Inventory.Instance.Select(Inventory.Instance.GetInventoryItems[i]);
                StartCoroutine(MoveItemImageToCursor(slots[i].transform.GetChild(0).GetComponent<Image>(), selectedSlot.gameObject));
            }

            slots[i].GetComponent<Image>().enabled = selectedSlot.gameObject == slots[i].gameObject;
        }
    }

    public void PreviewItem(ItemData item)
    {
        var image = SetItemUISprite(item);

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].gameObject.activeInHierarchy)
            {
                var coroutine = StartCoroutine(AddToInventoryUI(item, image, slots[i].transform));
                movingItems.Add(coroutine);
                return;
            }
        }
    }

    private IEnumerator AddToInventoryUI(ItemData item, Image image, Transform slot)
    {
        Inventory.Instance.CanAddItems = false;
        itemUIContainer.SetActive(true);
        image.gameObject.SetActive(true);
        slot.gameObject.SetActive(true);
        slot.GetComponent<Button>().enabled = false;
        
        if (item.ScaleInPreviewUI != Vector2.zero) image.rectTransform.sizeDelta = item.ScaleInPreviewUI;
        else Debug.Log("Please set item UI preview scale!");

        if(item.ScaleInInventoryUI == Vector2.zero) Debug.Log("Please set item UI inventory scale!");

        yield return StartCoroutine(GoToUIPosition(
            image: image,
            startScale: item.transform.localScale * 100,
            targetScale: item.ScaleInPreviewUI,
            startRotation: item.transform.rotation,
            targetRotation: Quaternion.Euler(item.RotationInPreviewUI),
            duration: goToPreviewDuration,
            refLerpTime: 0f,
            percentage: 0f,
            startPosition: new Vector2(
                _camera.ScreenToViewportPoint(Input.mousePosition).x * Screen.width - (Screen.width / 2),
                _camera.ScreenToViewportPoint(Input.mousePosition).y * Screen.height - (Screen.height / 2))
            ));

        yield return new WaitForSeconds(itemPreviewDuration);
        image.transform.SetParent(slot);

        yield return StartCoroutine(GoToUIPosition(
            image: image,
            startScale: item.ScaleInPreviewUI,
            targetScale: item.ScaleInInventoryUI,
            startRotation: image.transform.rotation,
            targetRotation: Quaternion.Euler(item.RotationInInventory),
            duration: goToSlotDuration,
            refLerpTime: 0f,
            percentage: 0f,
            startPosition: image.rectTransform.anchoredPosition
            ));

        itemUIContainer.SetActive(false);
        slot.GetComponent<Button>().enabled = true;

        if(movingItems.Count > 0) movingItems.RemoveAt(0); //Removes this coroutine
        if(movingItems.Count <= 0) Inventory.Instance.CanAddItems = true;

        Debug.Log("Finished moving inventory UI");
    }

    private IEnumerator GoToUIPosition(float percentage, float refLerpTime, Image image, Vector3 startPosition, Vector3 startScale, Vector3 targetScale, Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        while (percentage < 1)
        {
            percentage = Interpolation.Sinerp(duration, ref refLerpTime);
            image.rectTransform.anchoredPosition = Vector3.Lerp(startPosition, Vector3.zero, percentage);
            image.rectTransform.sizeDelta = Vector2.Lerp(startScale, targetScale, percentage);
            image.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percentage);
            yield return null;
        }
    }

    public IEnumerator RemoveInventoryItem()
    {
        yield return new WaitUntil(()=>movingItems.Count <= 0);
        var removed = false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (!removed && selectedSlot == slots[i])
            {
                Destroy(selectedSlot.transform.GetChild(0).gameObject);
                selectedSlot.GetComponent<Image>().enabled = false;
                removed = true;
            }

            if (removed)
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

    public IEnumerator MoveItemImageToCursor(Image image, GameObject slot)
    {
        var slotButton = slot.GetComponent<Button>();

        slotButton.enabled = false;
        image.raycastTarget = false;
        Cursor.visible = false;

        while (image && selectedSlot && slot == selectedSlot.gameObject)
        {
            var position = new Vector2(
                _camera.ScreenToViewportPoint(Input.mousePosition).x * Screen.width, 
                _camera.ScreenToViewportPoint(Input.mousePosition).y * Screen.height);

            image.transform.position = position;
            yield return null;
        }

        slotButton.enabled = true;

        if (image)
        {
            image.transform.position = slot.transform.position;
            image.raycastTarget = true;
        }

        Cursor.visible = true;
    }

    public void DeselectSlot()
    {
        if(selectedSlot)
        {
            selectedSlot.GetComponent<Image>().enabled = false;
            selectedSlot = null;
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