using SnowHorse.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private float goToPreviewDuration = 0.4f;
    [SerializeField] private float itemPreviewDuration = 0.2f;
    [SerializeField] private float goToSlotDuration = 0.6f;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject inventoryUIContainer;
    [SerializeField] private GameObject inventorySlotsContainer;
    [SerializeField] private GameObject inventoryUIBackground;
    [SerializeField] private GameObject itemPreviewUIContainer;
    [SerializeField] private Image itemImagePrefab;
    [SerializeField] private Button openInventoryButton;
    [SerializeField] private Button[] slots;

    private float goToPreviewMultiplier = 1000f;
    private Button selectedSlot;
    private int movingItemCount = 0;

    public int MovingItemCount { get => movingItemCount; }
    public Button[] GetSlots { get => slots; }

    private void Awake()
    {
        foreach(var slot in slots)
        {
            slot.onClick.AddListener(() => { SelectSlot(); });
            slot.GetComponent<Image>().enabled = false;
            slot.gameObject.SetActive(false);
        }

        inventoryUIContainer.SetActive(false);
    }

    private void Start()
    {
        openInventoryButton.onClick.AddListener(() =>
        {
            inventoryUIContainer.SetActive(!inventoryUIContainer.activeInHierarchy);
            Inventory.Instance.DeselectPreviousItem();
        });

        ModifyBackgroundWidth(false);
        goToPreviewDuration *= goToPreviewMultiplier;
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

    public void Add(Item item, bool previewOnUI, bool dataPersistenceMode = false)
    {
        inventoryUIContainer.SetActive(true);
        var image = SetItemUISprite(item);

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].gameObject.activeInHierarchy)
            {
                StartCoroutine(AddToInventoryUI(item, image, slots[i].transform, previewItem: previewOnUI, dataPersistenceMode));
                return;
            }
        }
    }

    private IEnumerator AddToInventoryUI(Item item, Image image, Transform slot, bool previewItem, bool dataPersistenceMode = false)
    {
        movingItemCount++;
        Debug.Log($"Started moving item to inventory UI, moving item count: {movingItemCount}");
        if (!dataPersistenceMode) itemPreviewUIContainer.SetActive(true);
        image.gameObject.SetActive(true);
        slot.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventorySlotsContainer.GetComponent<RectTransform>());
        slot.GetComponent<Button>().enabled = false;
        
        if (item.ScaleInPreviewUI != Vector2.zero) image.rectTransform.sizeDelta = item.ScaleInPreviewUI;
        else Debug.Log("Please set item UI preview scale!");

        if(item.ScaleInInventoryUI == Vector2.zero) Debug.Log("Please set item UI inventory scale!");
        ModifyBackgroundWidth(true);

        if(!previewItem) image.transform.SetParent(slot);
        var parentRect = image.transform.parent.GetComponent<RectTransform>();
        var imageRect = image.GetComponent<RectTransform>();
        var startScale = SpritePixelSize.Get(item.GetComponent<SpriteRenderer>(), _camera);
        var startPosition = new Vector2(
                    _camera.WorldToScreenPoint(item.gameObject.transform.position).x - parentRect.position.x,
                    _camera.WorldToScreenPoint(item.gameObject.transform.position).y - parentRect.position.y);

        if (dataPersistenceMode) previewItem = false;

        if (previewItem)
        {
            yield return StartCoroutine(GoToUIPosition(
                image: image,
                startScale: startScale,
                targetScale: item.ScaleInPreviewUI,
                startRotation: item.transform.rotation,
                targetRotation: Quaternion.Euler(item.RotationInPreviewUI),
                duration: goToPreviewDuration / imageRect.position.magnitude,
                refLerpTime: 0f,
                percentage: 0f,
                startPosition: startPosition));

            yield return new WaitForSecondsRealtime(itemPreviewDuration);
            image.transform.SetParent(slot);

            yield return StartCoroutine(GoToUIPosition(
                image: image,
                startScale: item.ScaleInPreviewUI,
                targetScale: item.ScaleInInventoryUI,
                startRotation: image.transform.rotation,
                targetRotation: Quaternion.Euler(new Vector3(0, 0, item.ZRotationInInventory)),
                duration: goToSlotDuration,
                refLerpTime: 0f,
                percentage: 0f,
                startPosition: image.rectTransform.anchoredPosition));
        }
        else
        {
            if(dataPersistenceMode)
            {
                image.rectTransform.anchoredPosition = Vector3.zero;
                image.rectTransform.sizeDelta = item.ScaleInInventoryUI;
                image.transform.rotation = Quaternion.Euler(new Vector3(0, 0, item.ZRotationInInventory));
            }
            else
            {
                yield return StartCoroutine(GoToUIPosition(
                    image: image,
                    startScale: startScale,
                    targetScale: item.ScaleInInventoryUI,
                    startRotation: image.transform.rotation,
                    targetRotation: Quaternion.Euler(new Vector3(0, 0, item.ZRotationInInventory)),
                    duration: goToSlotDuration,
                    refLerpTime: 0f,
                    percentage: 0f,
                    startPosition: startPosition));
            }
        }

        slot.GetComponent<Button>().enabled = true;
        if(movingItemCount > 0) movingItemCount--; //Removes this coroutine
        if(movingItemCount <= 0) itemPreviewUIContainer.SetActive(false);
        Debug.Log($"Finished moving item to inventory UI, moving item count: {movingItemCount}");
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
        yield return new WaitUntil(()=>movingItemCount <= 0);
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

                ModifyBackgroundWidth(false);
            }
        }
    }

    private void ModifyBackgroundWidth(bool isAddingItem)
    {
        var rectTransform = inventoryUIBackground.GetComponent<RectTransform>();
        var layoutGroup = inventorySlotsContainer.GetComponent<HorizontalLayoutGroup>();

        var countPadding = isAddingItem ? 1 : 0;

        if(Inventory.Instance.GetInventoryItems.Count > 0)
        {
            rectTransform.sizeDelta = new Vector2(
                slots[0].GetComponent<RectTransform>().sizeDelta.x * (Inventory.Instance.GetInventoryItems.Count + countPadding) +
                layoutGroup.spacing * (Inventory.Instance.GetInventoryItems.Count - 1) +
                layoutGroup.padding.right + layoutGroup.padding.left,
                rectTransform.sizeDelta.y);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(
                slots[0].GetComponent<RectTransform>().sizeDelta.x +
                layoutGroup.padding.right + layoutGroup.padding.left,
                rectTransform.sizeDelta.y);
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

    private Image SetItemUISprite(Item item, bool preserveAspect = true)
    {
        var image = Instantiate(itemImagePrefab, itemImagePrefab.transform.position, Quaternion.identity, itemImagePrefab.transform.parent);
        image.sprite = item.Sprite;
        image.color = item.Color;
        image.preserveAspect = preserveAspect;
        return image;
    }
}