using SnowHorse.Utils;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Camera _camera;

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
                StartCoroutine(MoveItemImageToCursor(slots[i].transform.GetChild(0).GetComponent<RectTransform>(), selectedSlot.gameObject));
            }

            slots[i].GetComponent<Image>().enabled = selectedSlot.gameObject == slots[i].gameObject;
        }
    }

    public void PreviewItem(ItemData item)
    {
        itemName.text = item.Name;
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
        itemName.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        slot.gameObject.SetActive(true);
        slot.GetComponent<Button>().enabled = false;
        
        if (item.ScaleInPreviewUI != Vector2.zero) image.rectTransform.sizeDelta = item.ScaleInPreviewUI;
        else Debug.Log("Please set item UI preview scale!");

        if(item.ScaleInInventoryUI == Vector2.zero) Debug.Log("Please set item UI inventory scale!");

        yield return new WaitForSeconds(itemPreviewDuration);

        image.transform.SetParent(slot);

        var startPosition = image.rectTransform.anchoredPosition;
        var startScale = item.ScaleInPreviewUI;
        var targetScale = item.ScaleInInventoryUI;
        var startRotation = image.transform.rotation;
        var targetRotation = Quaternion.Euler(item.RotationInInventory);
        var refLerpTime = 0f;
        var percentage = 0f;

        itemName.gameObject.SetActive(false);

        while (percentage < 1)
        {
            percentage = Interpolation.Sinerp(goToSlotDuration, ref refLerpTime);
            image.rectTransform.anchoredPosition = Vector3.Lerp(startPosition, Vector3.zero, percentage);
            image.rectTransform.sizeDelta = Vector2.Lerp(startScale, targetScale, percentage);
            image.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percentage);
            yield return null;
        }

        itemUIContainer.SetActive(false);
        slot.GetComponent<Button>().enabled = true;

        if(movingItems.Count > 0) movingItems.RemoveAt(0); //Removes this coroutine
        if(movingItems.Count <= 0) Inventory.Instance.CanAddItems = true;

        Debug.Log("Finished moving inventory UI");
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

    public IEnumerator MoveItemImageToCursor(RectTransform image, GameObject slot)
    {
        while(image && slot == selectedSlot.gameObject)
        {
            Cursor.visible = false;
            slot.GetComponent<Button>().enabled = false;
            image.GetComponent<Image>().raycastTarget = false;

            var position = new Vector2(
                _camera.ScreenToViewportPoint(Input.mousePosition).x * Screen.width, 
                _camera.ScreenToViewportPoint(Input.mousePosition).y * Screen.height);

            image.transform.position = position;
            yield return null;
        }

        Cursor.visible = true;
        slot.GetComponent<Button>().enabled = true;

        if (image)
        {
            image.transform.position = slot.transform.position;
            image.GetComponent<Image>().raycastTarget = true;
        } 
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