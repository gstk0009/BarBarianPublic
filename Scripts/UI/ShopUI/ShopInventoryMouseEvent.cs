using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopInventoryMouseEvent : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private ItemTooltipUI itemTooltipUI;

    private RectTransform rt;
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    private Canvas canvas;

    private ShopItemSlotUI pointerOverSlot;

    private int leftClick = 0;
    private int rightClick = 1;

    private int initGold = 0;
    private int afterGold = 0;
    public bool isBuying = false;
    private float lastBuyingTime = 0f;
    private float delayTime = 0.2f;

    [SerializeField] private bool showHighlight = true;
    [SerializeField] private bool showTooltip = true;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        inventory = Player.Instance.inventory;
    }

    private void Update()
    {
        ShopInventoryMouseEventUpdate();
    }

    public void ShopInventoryMouseEventUpdate()
    {
        ped.position = Input.mousePosition;

        OnPointerEnterAndExit();
        if (showTooltip) ShowOrHideItemTooltip();
        OnPointerDown();
    }

    private void Init()
    {
        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);
        rt = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();

        if (itemTooltipUI == null)
        {
            itemTooltipUI = GetComponent<ItemTooltipUI>();
        }
    }

    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        rrList.Clear();

        gr.Raycast(ped, rrList);

        if (rrList.Count == 0)
            return null;

        return rrList[0].gameObject.GetComponent<T>();
    }

    private void OnPointerEnterAndExit()
    {
        var prevSlot = pointerOverSlot;
        var curSlot = pointerOverSlot = RaycastAndGetFirstComponent<ShopItemSlotUI>();

        if (prevSlot == null)
        {
            if (curSlot != null)
            {
                OnCurrentEnter();
            }
        }
        else
        {
            if (curSlot == null)
            {
                OnPrevExit();
            }
            else if (prevSlot != curSlot)
            {
                OnPrevExit();
                OnCurrentEnter();
            }
        }

        void OnCurrentEnter()
        {
            if (showHighlight && curSlot.IsAccessible)
                curSlot.Highlight(true);
        }
        void OnPrevExit()
        {
            prevSlot.Highlight(false);
        }
    }

    private void ShowOrHideItemTooltip()
    {
        if (pointerOverSlot == null) return;

        bool isValid = pointerOverSlot != null && pointerOverSlot.IsAccessible;

        if (isValid)
        {
            UpdateTooltipUI(pointerOverSlot);
            itemTooltipUI.Show();
        }
        else
            itemTooltipUI.Hide();
    }

    private void UpdateTooltipUI(ShopItemSlotUI slot)
    {
        if (!slot.IsAccessible)
            return;

        itemTooltipUI.SetItemInfo(slot.itemData);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, canvas.worldCamera, out Vector2 localPoint);
        itemTooltipUI.SetMousePointPosition(localPoint);
    }

    private void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(leftClick) || Input.GetMouseButton(rightClick))
        {
            if (pointerOverSlot != null && pointerOverSlot.IsAccessible)
            {
                bool isSucceed = TryBuyItem(pointerOverSlot.itemData);
                if (isSucceed && pointerOverSlot.itemData is ShopItemData shopItemData && Buyingitemtype.SingleUse == shopItemData.BuyType)
                {
                    pointerOverSlot.SetItemAccessibleState(false);
                }
            }
        }
    }

    private bool TryBuyItem(ItemData data)
    {
        if (Time.time - lastBuyingTime < delayTime)
        {
            return false;
        }

        if (!isBuying)
        {
            lastBuyingTime = Time.time;
            initGold = DataManager.Instance.currentPlayer.gold;
            afterGold = initGold - data.BuyPrice;

            if ((BuyItem.Possible == data.Buy) && (afterGold >= 0))
            {
                int amount = inventory.Add(data);
                if (amount != 0)
                {
                    return false;
                }
                isBuying = true;
                GameManager.Instance.buyOrSellManager.Buy(initGold, afterGold, this);
                return true;
            }
        }
        return false;
    }
}
