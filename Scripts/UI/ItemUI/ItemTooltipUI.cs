using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI SellingPriceText;

    private RectTransform rt;
    private CanvasScaler scaler;

    private Vector2 leftTop = new Vector2(0, 1f);

    private void Awake()
    {
        Init();
        Hide();
    }

    private void Init()
    {
        rt = GetComponent<RectTransform>();
        rt.pivot = leftTop;
        scaler = GetComponentInParent<CanvasScaler>();
    }

    public void SetItemInfo(ItemData itemData)
    {
        if (itemData == null)
        {
            Hide();
            return;
        }

        titleText.text = itemData.Name;
        contentText.text = itemData.ToolTip;

        contentText.gameObject.SetActive(true);
        SellingPriceText.gameObject.SetActive(false);
    }

    public void SetItemSellingInfo(ItemSlotUIs slot)
    {
        if (slot == null)
        {
            Hide();
            return;
        }

        if (slot.itemData.Sell == SellItem.Possible)
        {
            if (Player.Instance.inventory.GetItem(slot.Index) is CountableItem countItem)
            {
                if (countItem != null)
                {
                    int amount = countItem.Amount;
                    titleText.text = slot.itemData.Name + $" x {amount}";
                    SellingPriceText.text = (slot.itemData.SellPrice * amount).ToString();
                }
                else
                {
                    titleText.text = slot.itemData.Name;
                    SellingPriceText.text = slot.itemData.SellPrice.ToString();
                }
            }
            else
            {
                titleText.text = slot.itemData.Name;
                SellingPriceText.text = slot.itemData.SellPrice.ToString();
            }
            contentText.gameObject.SetActive(false);
            SellingPriceText.gameObject.SetActive(true);
        }
    }

    
    public void SetItemInfoForEnchant(int idx)
    {
        Item item = Player.Instance.inventory.GetItem(idx);
        if (item == null)
        {
            Hide();
            return;
        }
        
        if(item is  WeaponItem weaponItem) 
        {
            titleText.text = weaponItem.ItemName;
            if (weaponItem.EnhanceRate > 0)
            { 
                contentText.text = $"STR : {weaponItem.WeaponItemStr}<color=#6CF6FF>  (+{weaponItem.EnhanceRate})</color>";
            }
            else
            {
                contentText.text = $"STR : {weaponItem.WeaponItemStr}";
            }

        }
        if (item is ArmorItem armorItem)
        {
            titleText.text = armorItem.ItemName;
            if (armorItem.EnhanceRate > 0)
            {
                contentText.text = $"DEF : {armorItem.ArmorItemDef}<color=#6CF6FF>  (+{armorItem.EnhanceRate})</color>";
            }
            else
            {
                contentText.text = $"DEF : {armorItem.ArmorItemDef}";
            }

        }
        else if (item is MaterialItem materialItem)
        {
            ItemData id = Player.Instance.inventory.GetItemData(idx);
            titleText.text = id.Name;
            contentText.text = id.ToolTip;
        }

        contentText.gameObject.SetActive(true);

        if(SellingPriceText != null)
            SellingPriceText.gameObject.SetActive(false);
    }

    public void SetRectPosition(RectTransform slotRect, float minAnchorX)
    {
        float wRatio = Screen.width / scaler.referenceResolution.x;
        float hRatio = Screen.height / scaler.referenceResolution.y;

        float ratio = wRatio * (1f - scaler.matchWidthOrHeight) + hRatio * (scaler.matchWidthOrHeight);

        float slotWidth = slotRect.rect.width * ratio;
        float slotHeight = slotRect.rect.height * ratio;

        float width = rt.rect.width * ratio;
        float height = rt.rect.height * ratio;

        rt.anchoredPosition = new Vector2(slotRect.anchoredPosition.x + slotWidth, slotRect.anchoredPosition .y + slotHeight - 50f);

        bool rightTruncated = rt.anchoredPosition.x > minAnchorX;

        if (rightTruncated)
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x - width - slotWidth, rt.anchoredPosition.y);
    }

    public void SetMousePointPosition(Vector2 mousePoint)
    {
        rt.anchoredPosition = new Vector2(mousePoint.x + 20f, mousePoint.y - 20f);

        bool rightTruncated = (rt.anchoredPosition.x > 365f);
        bool leftTruncated = (rt.anchoredPosition.x < -800f);
        bool bottomTurncated = (rt.anchoredPosition.y < -140f);

        if (rightTruncated)
            rt.anchoredPosition = new Vector2(365f, rt.anchoredPosition.y);
        else if (leftTruncated)
            rt.anchoredPosition = new Vector2(-800f, rt.anchoredPosition.y);
        else if (bottomTurncated)
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -140f);
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
