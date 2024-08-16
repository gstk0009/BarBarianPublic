using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellInventoryMouseEvent : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private ItemTooltipUI itemTooltipUI;

    private RectTransform rt;
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    private Canvas canvas;

    private ItemSlotUIs pointerOverSlot;

    private int leftClick = 0;
    private int rightClick = 1;

    private int initGold = 0;
    private int afterGold = 0;
    public bool isSelling = false;

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
        SellingInventoryMouseEventUpdate();
    }

    public void SellingInventoryMouseEventUpdate()
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
        var curSlot = pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUIs>();

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
            if (showHighlight && curSlot.HasItem && curSlot.IsAccessible && curSlot.itemData.Sell == SellItem.Possible)
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

        bool isValid = pointerOverSlot != null && pointerOverSlot.IsAccessible && pointerOverSlot.HasItem
            && pointerOverSlot.itemData.Sell == SellItem.Possible;

        if (isValid)
        {
            UpdateTooltipUI(pointerOverSlot);
            itemTooltipUI.Show();
        }
        else
            itemTooltipUI.Hide();
    }

    private void UpdateTooltipUI(ItemSlotUIs slot)
    {
        if (!slot.IsAccessible || !slot.HasItem)
            return;

        itemTooltipUI.SetItemSellingInfo(slot);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, canvas.worldCamera, out Vector2 localPoint);
        itemTooltipUI.SetMousePointPosition(localPoint);
    }

    private void OnPointerDown()
    {
            if (Input.GetMouseButtonDown(leftClick))
        {
                if (pointerOverSlot != null && pointerOverSlot.HasItem && pointerOverSlot.IsAccessible && SellItem.Possible == pointerOverSlot.itemData.Sell)
                {
                    TrySellItem(pointerOverSlot);
                }
            }
            else if (Input.GetMouseButtonDown(rightClick))
            {
                if (pointerOverSlot != null && pointerOverSlot.HasItem && pointerOverSlot.IsAccessible && SellItem.Possible == pointerOverSlot.itemData.Sell)
                {
                    TrySellAllItem(pointerOverSlot);
                }
            }
    }

    private void TrySellItem(ItemSlotUIs slot)
    {
        if (!isSelling)
        {
            initGold = DataManager.Instance.currentPlayer.gold;
            afterGold = initGold + slot.itemData.SellPrice;
            isSelling = true;
            GameManager.Instance.buyOrSellManager.Sell(initGold, afterGold, this);
            if (Player.Instance.inventory.GetItem(slot.Index) is CountableItem countableItem)
            {
                TrySellCountableItem(countableItem, slot);
            }
            else
            {
                inventory.Remove(slot.Index);
            }
        }
    }

    private void TrySellAllItem(ItemSlotUIs slot)
    {
        if (!isSelling)
        {
            initGold = DataManager.Instance.currentPlayer.gold;
            afterGold = initGold + slot.itemData.SellPrice;
            if (Player.Instance.inventory.GetItem(slot.Index) is CountableItem countableItem)
            {
                int amount = countableItem.Amount;
                afterGold = initGold + (slot.itemData.SellPrice * amount);
                countableItem.SubtractAmount(amount);
            }
            isSelling = true;
            GameManager.Instance.buyOrSellManager.Sell(initGold, afterGold, this);
            inventory.Remove(slot.Index);
        }
    }

    private void TrySellCountableItem(CountableItem countableItem , ItemSlotUIs slot)
    {
        if (countableItem == null) return;

        countableItem.SubtractAmount(1);
        inventory.UpdateSlot(slot.Index);

        if (countableItem.Amount == 0)
            inventory.Remove(slot.Index);
    }
}
