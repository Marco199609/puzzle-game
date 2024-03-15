using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;

    [Header("Inventory")]
    [SerializeField] private float itemPreviewDuration = 1.0f;
    [SerializeField] private GameObject itemUIContainer;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private List<Image> slots;

    [Header("Cursor")]
    [SerializeField] private Texture2D cross;
    [SerializeField] private Texture2D circle;

    public static UIController Instance;
    private void Awake()
    {
        if(Instance) Destroy(this);
        else Instance = this;
    }

    public void ShowItem(ItemData item)
    {
        itemName.text = item.Name;
        SetItemUISprite(itemImage, item);

        StartCoroutine(AddToInventoryUI(item));
    }

    private IEnumerator AddToInventoryUI(ItemData item)
    {
        itemUIContainer.SetActive(true);

        yield return new WaitForSeconds(itemPreviewDuration);

        itemUIContainer.SetActive(false);
        slots[0].gameObject.SetActive(true);
        SetItemUISprite(slots[0], item);
    }

    private void SetItemUISprite(Image image, ItemData item, bool preserveAspect = true)
    {
        image.sprite = item.Sprite;
        image.color = item.Color;
        image.preserveAspect = preserveAspect;
    }

    public void SetCursor(bool canInteract)
    {
        var cursor = canInteract ? circle : cross;
        Cursor.SetCursor(cursor, new Vector2(cursor.height / 2, cursor.width / 2), CursorMode.Auto);
    }
}