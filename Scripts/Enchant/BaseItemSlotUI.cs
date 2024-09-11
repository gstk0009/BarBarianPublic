using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseItemSlotUI : MonoBehaviour
{
    [SerializeField] protected float padding = 1f;
    [SerializeField] public Image iconImage;
    [SerializeField] protected TextMeshProUGUI amountText;
    [SerializeField] protected Image highlightImage;
    [SerializeField] protected float highlighAlpha = 0.5f;
    [SerializeField] protected float highlightFadeDuration = 0.2f;
    [SerializeField] protected Image guideIconImage;
    [SerializeField] protected Image TypeBackgroundIamge;

    public int Index { get; private set; }
    public bool HasItem => iconImage.sprite != null;
    public bool IsAccessible => isAccessibleSlot && isAccessibleItem;
    public RectTransform SlotRect => slotRect;
    public RectTransform IconRect => iconRect;

    protected RectTransform slotRect;
    protected RectTransform iconRect;
    protected RectTransform highlightRect;

    protected GameObject iconGo;
    protected GameObject textGo;
    protected GameObject highlightGo;
    protected GameObject TypebackgroundGo;

    protected Image slotImage;

    protected float currentHLAlpa = 0f;

    protected bool isAccessibleSlot = true;
    protected bool isAccessibleItem = true;

    protected static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    protected static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    // ItemData 추가
    public ItemData itemData;

    // 수량 정보 추가
    public int itemAmount;

    public abstract bool SwapOrMoveIcon(BaseItemSlotUI other);
    public abstract bool IsRightSlot(ItemData data);

    public abstract void SetItem(Sprite itemSprite, ItemData data);

    protected virtual void Awake()
    {
        InitComponents();
        InitValue();
    }

    protected void InitComponents()
    {
        slotRect = GetComponent<RectTransform>();
        iconRect = iconImage.rectTransform;
        highlightRect = highlightImage.rectTransform;

        iconGo = iconRect.gameObject;
        textGo = amountText.gameObject;
        highlightGo = highlightImage.gameObject;
        if (TypeBackgroundIamge != null)
            TypebackgroundGo = TypeBackgroundIamge.gameObject;

        slotImage = GetComponent<Image>();
    }

    protected void InitValue()
    {
        iconRect.pivot = new Vector2(0.5f, 0.5f);
        iconRect.anchorMin = Vector2.zero;
        iconRect.anchorMax = Vector2.one;

        iconRect.offsetMin = Vector2.one * padding;
        iconRect.offsetMax = Vector2.one * -padding;

        highlightRect.pivot = iconRect.pivot;
        highlightRect.anchorMin = iconRect.anchorMin;
        highlightRect.anchorMax = iconRect.anchorMax;
        highlightRect.offsetMin = iconRect.offsetMin;
        highlightRect.offsetMax = iconRect.offsetMax;

        iconImage.raycastTarget = false;
        highlightImage.raycastTarget = false;
        
        HideIcon();
        highlightGo.SetActive(false);

        if (TypeBackgroundIamge != null)
        {
            TypeBackgroundIamge.raycastTarget = false;
            TypebackgroundGo.SetActive(false);
        }
    }

    public void SetSlotsAccessibleState(bool value)
    {
        if (isAccessibleSlot == value) return;

        if (value)
        {
            slotImage.color = Color.black;
        }
        else
        {
            slotImage.color = InaccessibleSlotColor;
            HideIcon();
            HideText();
        }

        isAccessibleSlot = value;
    }

    public void SetItemAccessibleState(bool value)
    {
        if (isAccessibleItem == value) return;

        if (value)
        {
            iconImage.color = Color.white;
            amountText.color = Color.white;
        }
        else
        {
            iconImage.color = InaccessibleIconColor;
            amountText.color = InaccessibleIconColor;
        }

        isAccessibleItem = value;
        isAccessibleSlot = value;
    }

    public virtual void RemoveItem()
    {
        if (iconImage != null)
            iconImage.sprite = null;
        itemData = null;
        itemAmount = 0;

        HideIcon();
        HideText();
    }

    public void SetItemAmount(int amount)
    {
        itemAmount = amount;
        if (HasItem && amount > 1)
        {
            ShowText();
            amountText.text = amount.ToString();
        }
        else
        {
            HideText();
        }
    }

    public int GetItemAmount()
    {
        return itemAmount;
    }

    public bool DecreaseItem(int amount)
    {
        bool isPossible = (itemAmount - amount) >= 0;
        if (isPossible)
        {
            itemAmount -= amount;
            SetItemAmount(itemAmount);  // 수량 업데이트 후 바로 UI 업데이트
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Highlight(bool show)
    {
        if (!this.IsAccessible) return;

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

    protected void hideGuideIconImage(BaseItemSlotUI other)
    {
        if (other.guideIconImage != null)
            other.guideIconImage.gameObject.SetActive(false);
    }

    public void showGuideIconImage()
    {
        guideIconImage.gameObject.SetActive(true);
    }

    protected void ShowIcon() => iconGo.SetActive(true);
    protected void HideIcon() => iconGo.SetActive(false);
    protected void ShowText() => textGo.SetActive(true);
    protected void HideText() => textGo.SetActive(false);
    public void SetSlotIndex(int slotIndex) => Index = slotIndex;
}
