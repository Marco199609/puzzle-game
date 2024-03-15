using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject itemUIContainer;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;

    public static UIController Instance;
    private void Awake()
    {
        if(Instance) Destroy(this);
        else Instance = this;
    }

    public void ShowItem(ItemData item)
    {
        itemName.text = item.Name;
        itemImage.sprite = item.Sprite;
        itemImage.color = item.color;
        itemImage.preserveAspect = true;

        itemUIContainer.SetActive(true);
    }
}