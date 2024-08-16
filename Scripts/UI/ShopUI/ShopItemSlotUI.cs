using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlotUI : MonoBehaviour
{
    public ItemData itemData;

    [SerializeField] public Image iconImage;
    [SerializeField] public TextMeshProUGUI ItemNameTxt;
    [SerializeField] public TextMeshProUGUI ItemBuyingGoldTxt;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Image BuyingGoldImage;

    public bool IsAccessible = true;
    private RectTransform highlightRect;
    private GameObject highlightGo;

    private Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        highlightRect = highlightImage.rectTransform;
        highlightGo = highlightImage.gameObject;

        iconImage.raycastTarget = false;
        highlightImage.raycastTarget = false;
        ItemNameTxt.raycastTarget = false;
        ItemBuyingGoldTxt.raycastTarget = false;
        BuyingGoldImage.raycastTarget = false;
    }

    public void SetItem(ItemData data)
    {
        if (BuyItem.Possible == data.Buy)
        {
            itemData = data;
            iconImage.sprite = itemData.IconSprite;
            ItemNameTxt.text = itemData.Name;
            ItemBuyingGoldTxt.text = itemData.BuyPrice.ToString();
        }
    }


    public void Highlight(bool show)
    {
        if (show)
            highlightGo.SetActive(true);
        else
            highlightGo.SetActive(false);
    }

    public void SetHighlightOnTop(bool value)
    {
        if (value)
            highlightRect.SetAsLastSibling();
        else
            highlightRect.SetAsFirstSibling();
    }

    public void SetItemAccessibleState(bool value)
    {
        if (IsAccessible == value) return;

        if (value)
        {
            iconImage.color = Color.white;
            ItemNameTxt.color = Color.white;
            ItemBuyingGoldTxt.color = Color.white;
            BuyingGoldImage.color = Color.white;
        }
        else
        {
            iconImage.color = InaccessibleIconColor;
            ItemNameTxt.color = InaccessibleIconColor;
            ItemBuyingGoldTxt.color = InaccessibleIconColor;
            BuyingGoldImage.color = InaccessibleIconColor;
        }

        IsAccessible = value;
    }
}
