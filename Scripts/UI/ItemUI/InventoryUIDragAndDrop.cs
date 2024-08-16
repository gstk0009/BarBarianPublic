using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIDragAndDrop : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private ItemTooltipUI itemTooltipUI;
    [SerializeField] private InventoryPopupUI popupUI;

    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    private BaseItemSlotUI pointerOverSlot;
    private BaseItemSlotUI beginDragSlot;
    private Transform beginDragIconTransform;

    private int leftClick = 0;
    private int rightClick = 1;

    [SerializeField] private bool showHighlight = true;
    [SerializeField] private bool showRemovingPopup = true;
    [SerializeField] private bool showTooltip = true;

    // 스크린 좌표 -> 캔버스 좌표 변환을 위한 변수들 
    private Vector3 beginDragIconPoint;
    private Vector3 beginDragCursorPoint;
    private Vector3 originalIconScale;

    private int originalImageSortingOrder = 0;
    private int beginDragSlotSiblingIndex;
    private int originalSortingOrder;
    
    private Canvas beginDragCanvas;
    private Image draggedImage;

    private bool isDragging = false;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        DragAndDropUpdate();
    }

    public void DragAndDropUpdate()
    {
        ped.position = Input.mousePosition;

        OnPointerEnterAndExit();
        if (showTooltip) ShowOrHideItemTooltip();
        OnPointerDown();
        if (isDragging) OnPointerDrag(); // 드래그 상태라면 아이템 위치 업데이트
        OnPointerUp();
    }

    public void SetInventoryReference(Inventory inventory)
    {
        this.inventory = inventory;
    }

    private bool IsOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    private void Init()
    {
        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);

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

    private void ShowOrHideItemTooltip()
    {
        //if (pointerOverSlot == null) return;

        bool isValid = pointerOverSlot != null && pointerOverSlot.HasItem && pointerOverSlot.IsAccessible && (pointerOverSlot != beginDragSlot);

        if (isValid)
        {
            if (pointerOverSlot.Index < 33)
            {
                UpdateTooltipUI(pointerOverSlot);
                itemTooltipUI.Show();
            }
        }
        else
            itemTooltipUI.Hide();
    }

    private void UpdateTooltipUI(BaseItemSlotUI slot)
    {
        if (!slot.IsAccessible || !slot.HasItem)
            return;

        if (Player.Instance.inventory.GetItem(slot.Index) is EquipmentItem equipment)
        {
            if (equipment != null)
            {
                itemTooltipUI.SetItemInfoForEnchant(slot.Index);
            }
        }
        else
        {
            itemTooltipUI.SetItemInfo(inventory.GetItemData(slot.Index));
        }

        itemTooltipUI.SetRectPosition(slot.SlotRect, 340f);
    }

    private void OnPointerEnterAndExit()
    {
        var prevSlot = pointerOverSlot;
        var curSlot = pointerOverSlot = RaycastAndGetFirstComponent<BaseItemSlotUI>();

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
            else
            {
                OnPrevExit();
                OnCurrentEnter();
            }
        }

        void OnCurrentEnter()
        {
            if (showHighlight && curSlot.HasItem)
                curSlot.Highlight(true);
        }
        void OnPrevExit()
        {
            prevSlot.Highlight(false);
        }
    }

    private void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(leftClick))
        {
            beginDragSlot = RaycastAndGetFirstComponent<BaseItemSlotUI>();

            if (beginDragSlot != null && beginDragSlot.HasItem)
            {
                beginDragIconTransform = beginDragSlot.IconRect.transform;
                beginDragIconPoint = beginDragIconTransform.position;
                beginDragCursorPoint = Input.mousePosition;

                originalIconScale = beginDragIconTransform.localScale;
                beginDragSlotSiblingIndex = beginDragSlot.transform.GetSiblingIndex();
                beginDragSlot.transform.SetAsLastSibling();

                // Canvas 설정
                beginDragCanvas = beginDragSlot.IconRect.GetComponentInParent<Canvas>();
                if (beginDragCanvas != null)
                {
                    originalSortingOrder = beginDragCanvas.sortingOrder;
                    beginDragCanvas.overrideSorting = true;
                    beginDragCanvas.sortingOrder = 202;
                }

                // Image 설정
                draggedImage = beginDragSlot.IconRect.GetComponent<Image>();
                if (draggedImage != null)
                {
                    originalImageSortingOrder = draggedImage.canvas.sortingOrder;
                    draggedImage.canvas.overrideSorting = true;
                    draggedImage.canvas.sortingOrder = 202;
                }

                beginDragSlot.SetHighlightOnTop(false);

                isDragging = true; // 드래그 시작
            }
            else
            {
                beginDragSlot = null;
            }
        }
        else if (Input.GetMouseButtonDown(rightClick))
        {
            BaseItemSlotUI slot = RaycastAndGetFirstComponent<BaseItemSlotUI>();

            if (slot != null && slot.HasItem && slot.IsAccessible)
            {
                TryUseItem(slot.Index);
            }
        }
    }

    private void OnPointerDrag()
    {
        if (beginDragSlot == null) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(beginDragSlot.IconRect.parent as RectTransform, mousePosition, beginDragCanvas.worldCamera, out Vector2 localPoint);
            beginDragIconTransform.localPosition = localPoint;
        }
    }

    private void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(leftClick))
        {
            if (beginDragSlot != null)
            {
                beginDragIconTransform.position = beginDragIconPoint;
                beginDragIconTransform.localScale = originalIconScale;

                // Canvas 설정 복원
                if (beginDragCanvas != null)
                {
                    beginDragCanvas.sortingOrder = originalSortingOrder;
                    beginDragCanvas.overrideSorting = false;
                    beginDragCanvas = null;
                }

                // Image 설정 복원
                if (draggedImage != null)
                {
                    draggedImage.canvas.sortingOrder = originalImageSortingOrder;
                    draggedImage.canvas.overrideSorting = false;
                    draggedImage = null;
                }

                beginDragSlot.transform.SetSiblingIndex(beginDragSlotSiblingIndex);

                EndDrag();

                beginDragSlot = null;
                beginDragIconTransform = null;
                isDragging = false; // 드래그 종료
            }
        }
    }

    private void EndDrag()
    {
        BaseItemSlotUI endDragSlot = RaycastAndGetFirstComponent<BaseItemSlotUI>();

        if (endDragSlot != null && endDragSlot.IsAccessible)
        {
            bool isSeparatable = (Input.GetKey(KeyCode.LeftShift) && inventory.IsCountableItem(beginDragSlot.Index) && !inventory.HasItem(endDragSlot.Index));

            bool isSeparation = false;
            int currentAmount = 0;

            if (isSeparatable)
            {
                currentAmount = inventory.GetCurrentAmount(beginDragSlot.Index);
                if (currentAmount > 1)
                {
                    isSeparation = true;
                }
            }

            if (isSeparation)
                TrySeparateAmount(beginDragSlot.Index, endDragSlot.Index, currentAmount);
            else
                TrySwapItems(beginDragSlot, endDragSlot); // 아이템 분리 작업이 아니면 swap 시도

            if (endDragSlot.Index < 33)
                UpdateTooltipUI(endDragSlot);
            return;
        }

        if (IsOverUI())
        {
            int index = beginDragSlot.Index;
            string itemName = inventory.GetItemName(index);
            int amount = inventory.GetCurrentAmount(index);

            if (amount > 1)
                itemName += $"x{amount}";

            if (showRemovingPopup)
            {
                popupUI.OpenConfirmationPopup(itemName, index);
            }
            else
                TryRemoveItem(index);
        }
    }

    private void TrySeparateAmount(int indexA, int indexB, int amount)
    {
        if (indexA == indexB) return;

        string itemName = $"{inventory.GetItemName(indexA)} x{amount}";
        popupUI.OpenAmountInputPopup(itemName, amount, indexA, indexB);
    }

    public void TryRemoveItem(int index)
    {
        inventory.Remove(index);
    }

    private void TryUseItem(int index)
    {
        inventory.Use(index);
    }

    public void TryInventorySeparateAmount(int indexA, int indexB, int amt)
    {
        inventory.SeparateAmount(indexA, indexB, amt);
    }

    private void TrySwapItems(BaseItemSlotUI from, BaseItemSlotUI to)
    {
        if (from == to)
        {
            return;
        }

        // 장비 슬롯 간의 스왑을 확인
        if ((from is ItemSlotUIs fromItemSlot && to is ItemSlotUIs toItemSlot)
            && (fromItemSlot.isEquipmentArmor || fromItemSlot.isEquipmentHat || fromItemSlot.isEquipmentRing || fromItemSlot.isEquipmentShield || fromItemSlot.isEquipmentWeapon)
            && (toItemSlot.isEquipmentArmor || toItemSlot.isEquipmentHat || toItemSlot.isEquipmentRing || toItemSlot.isEquipmentShield || toItemSlot.isEquipmentWeapon))
        {
            if (from.SwapOrMoveIcon(to))
            {
                inventory.Swap(from.Index, to.Index);
            }
            else { }
        }
        // 인벤토리 슬롯 간의 스왑을 확인
        else if (!(from is ItemSlotUIs fromInventorySlot && to is ItemSlotUIs toInventorySlot)
            || (!fromInventorySlot.isEquipmentArmor && !fromInventorySlot.isEquipmentHat && !fromInventorySlot.isEquipmentRing && !fromInventorySlot.isEquipmentShield && !fromInventorySlot.isEquipmentWeapon)
            || (!toInventorySlot.isEquipmentArmor && !toInventorySlot.isEquipmentHat && !toInventorySlot.isEquipmentRing && !toInventorySlot.isEquipmentShield && !toInventorySlot.isEquipmentWeapon))
        {
            if (from.SwapOrMoveIcon(to))
            {
                inventory.Swap(from.Index, to.Index);
            }
        }
        else { }
    }
}
