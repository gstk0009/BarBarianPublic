using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnchantDragAndDrop : MonoBehaviour
{
    public static EnchantDragAndDrop instance;

    // 차례대로 강화 대상, 강화 재료, 강화 결과 item에 대한 정보 저장
    public Item[] enchantSlotItems = new Item[3];

    [SerializeField] private EnchantMenuUI enchantUI;
    [SerializeField] private ItemTooltipUI itemTooltipUI;
    [SerializeField] private bool showTooltip = true;

    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    private BaseItemSlotUI pointerOverSlot;
    private BaseItemSlotUI beginDragSlot;
    private Transform beginDragIconTransform;
    private Inventory inventory;

    private int leftClick = 0;

    // 스크린 좌표 -> 캔버스 좌표 변환을 위한 변수들 
    private Vector3 beginDragIconPoint;
    private Vector3 beginDragCursorPoint;
    private Vector3 originalIconScale;

    private int originalImageSortingOrder = 0;
    private int beginDragSlotSiblingIndex;

    private Canvas beginDragCanvas;
    private Image draggedImage;

    private bool isDragging = false;

    int index = -1;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
       DragAndDropUpdate();
    }

    private void Init()
    {
        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);
        inventory = Player.Instance.inventory;
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

    private void UpdateTooltipUI(BaseItemSlotUI slot)
    {
        if (!slot.IsAccessible || !slot.HasItem || itemTooltipUI == null)
            return;

        if (slot is EnchantItemSlot)
            return;
       
        itemTooltipUI.SetItemInfoForEnchant(slot.Index);
        itemTooltipUI.SetRectPosition(slot.SlotRect, 860f);
    }

    private void ShowOrHideItemTooltip()
    {
        bool isValid = pointerOverSlot != null && pointerOverSlot.HasItem && pointerOverSlot.IsAccessible && (pointerOverSlot != beginDragSlot)
            && !(pointerOverSlot is EnchantItemSlot);

        if (isValid)
        {
            if (pointerOverSlot.Index < 33)
            {
                UpdateTooltipUI(pointerOverSlot);
                itemTooltipUI.Show();
            }
        }
        else
        {
            itemTooltipUI.Hide();
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
        var curSlot = pointerOverSlot = RaycastAndGetFirstComponent<BaseItemSlotUI>();

        if (prevSlot == null && curSlot != null)
        {
            OnCurrentEnter();
        }
        else if (prevSlot != null && curSlot == null)
        {
            OnPrevExit();
        }
        else if (prevSlot != curSlot)
        {
            OnPrevExit();
            OnCurrentEnter();
        }

        void OnCurrentEnter()
        {
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
            if (beginDragSlot != null && beginDragSlot.IsAccessible && beginDragSlot.HasItem)
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
                    originalImageSortingOrder = beginDragCanvas.sortingOrder;
                    beginDragCanvas.overrideSorting = true;
                }

                // Image 설정
                draggedImage = beginDragSlot.IconRect.GetComponent<Image>();
                if (draggedImage != null)
                {
                    originalImageSortingOrder = draggedImage.canvas.sortingOrder;
                    draggedImage.canvas.overrideSorting = true;
                }

                isDragging = true; // 드래그 시작
            }
            else
            {
                beginDragSlot = null;
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
        if (Input.GetMouseButtonUp(0))
        {
            if (beginDragSlot != null)
            {
                beginDragIconTransform.position = beginDragIconPoint;
                beginDragSlot.transform.SetSiblingIndex(beginDragSlotSiblingIndex);

                // Canvas 설정 복원
                if (beginDragCanvas != null)
                {
                    beginDragCanvas.sortingOrder = originalImageSortingOrder;
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

        if (beginDragSlot == null || endDragSlot == null)
        {
            ReturnToOriginalSlot();
            return;
        }

        if (endDragSlot.IsAccessible && endDragSlot != beginDragSlot)
        {
            if (beginDragSlot is EnchantItemSlot enchantBeginSlot && endDragSlot is ItemSlotUIs itemSlot)
            {
                if (itemSlot.IsRightSlot(enchantBeginSlot.itemData) && enchantBeginSlot.IsRightSlot(itemSlot.itemData))
                {
                    if (enchantBeginSlot.isWeaponSlot)
                    {
                        index = 0;
                    }
                    else if (enchantBeginSlot.isMaterialSlot)
                    {
                        index = 1;
                    }

                    // 교환 작업 - 유사 swap 작업
                    if (itemSlot.HasItem)
                    {
                        if (!EnchantItemSlot.invalidSlotAttempt) // 유효한 교환 시도일 경우, 
                        {
                            Item endDragSlotitem = Player.Instance.inventory.GetItem(endDragSlot.Index);
                            Item beginDragSlotItem = enchantSlotItems[index];
                            int BeginDragSlotAmount = enchantBeginSlot.GetItemAmount();

                            // === 인첸트 슬롯에 대한 할당 작업 
                            enchantBeginSlot.SetItem(itemSlot.iconImage.sprite, itemSlot.itemData);
                            enchantBeginSlot.SetItemAmount(itemSlot.GetItemAmount()); // 여기서 수량을 보여줌
                            enchantSlotItems[index] = endDragSlotitem;


                            // === 아이템 슬롯에 대한 할당 작업
                            itemSlot.SetItem(beginDragSlotItem.Data.IconSprite, beginDragSlotItem.Data);
                            itemSlot.SetItemAmount(BeginDragSlotAmount);
                            Player.Instance.inventory.AddItemDirectly(beginDragSlotItem, endDragSlot.Index);
                        }
                        else
                        {
                            ReturnToOriginalSlot();
                        }
                    }
                    else
                    {
                        itemSlot.SetItem(enchantSlotItems[index].Data.IconSprite, enchantSlotItems[index].Data);

                        int amount = enchantBeginSlot.GetItemAmount();
                        itemSlot.SetItemAmount(amount);
                        Player.Instance.inventory.AddItemDirectly(enchantSlotItems[index], endDragSlot.Index, amount); // 새로 정보 추가 
                        
                        enchantBeginSlot.SetItemAmount(0); // 비어있는 슬롯의 양을 0으로 할당

                        enchantBeginSlot.RemoveItem();
                        enchantSlotItems[index] = null;
                    }
                }
                else
                {
                    ReturnToOriginalSlot();
                }
            }
            else if (beginDragSlot is ItemSlotUIs beginItemSlot && endDragSlot is EnchantItemSlot enchantSlot)
            {
                if (enchantSlot.IsRightSlot(beginItemSlot.itemData))
                {
                    Item beginDragSlotItem = Player.Instance.inventory.GetItem(beginItemSlot.Index);
                    int beginDragSlotAmount = beginItemSlot.GetItemAmount();

                    if (enchantSlot.isWeaponSlot)
                    {
                        index = 0;
                    }
                    else if (enchantSlot.isMaterialSlot)
                    {
                        index = 1;
                    }

                    // 아이템 슬롯과 강화 슬롯간의 swap 작업
                    if (enchantSlot.HasItem)
                    {
                        if (!EnchantItemSlot.invalidSlotAttempt) // 유효한 swap 작업일 경우, 
                        {
                            // itemSlotUIs에서 드래그를 시작한 inventory 아이템을 삭제 
                            Player.Instance.inventory.Remove(beginDragSlot.Index);

                            // 새로운 정보를 할당 
                            beginItemSlot.SetItem(enchantSlot.iconImage.sprite, enchantSlot.itemData);
                            beginItemSlot.SetItemAmount(enchantSlot.GetItemAmount());
                            Player.Instance.inventory.AddItemDirectly(enchantSlotItems[index], beginDragSlot.Index);

                            // 인첸트 슬롯에는 beginDragSlot에 있던 정보로 갱신
                            enchantSlot.SetItem(beginDragSlotItem.Data.IconSprite, beginDragSlotItem.Data);
                            enchantSlot.SetItemAmount(beginDragSlotAmount);
                            enchantSlotItems[index] = beginDragSlotItem;
                        }
                        else
                        {
                            ReturnToOriginalSlot();
                        }
                    }
                    else // 단순 할당
                    {
                        enchantSlot.SetItem(beginDragSlotItem.Data.IconSprite, beginDragSlotItem.Data);
                        enchantSlot.SetItemAmount(beginDragSlotAmount);
                        enchantSlotItems[index] = beginDragSlotItem;

                        beginItemSlot.SetItemAmount(0);
                        beginItemSlot.RemoveItem(); // sprite, ItemSO에 대한 정보 삭제 
                        Player.Instance.inventory.Remove(beginDragSlot.Index); 
                    }
                }
                else
                {
                    ReturnToOriginalSlot();
                }
            }
            // EnchantItemSlot과 EnchantItemSlot 간 이동
            else if (beginDragSlot is EnchantItemSlot beginEnchantSlot && endDragSlot is EnchantItemSlot endEnchantSlot)
            {
                ReturnToOriginalSlot();
            }
            // ItemSlotUIs와 ItemSlotUIs 간 이동
            else if (beginDragSlot is ItemSlotUIs beginSlot && endDragSlot is ItemSlotUIs endSlot)
            {
                Item item = Player.Instance.inventory.GetItem(beginSlot.Index);
                int beginItemAmount = beginSlot.GetItemAmount();

                // 빈 슬롯에 대한 이동이라면, 
                if (!endSlot.HasItem)
                {
                    endDragSlot.SetItem(beginSlot.iconImage.sprite, beginSlot.itemData);
                    endDragSlot.SetItemAmount(beginItemAmount);
                    Player.Instance.inventory.AddItemDirectly(item, endSlot.Index);

                    // 드래그 시작 점의 정보를 삭제
                    beginSlot.RemoveItem(); // sprite, ItemSO에 대한 정보 삭제 
                    Player.Instance.inventory.Remove(beginDragSlot.Index);

                }
                else
                {
                    inventory.Swap(beginSlot.Index, endSlot.Index);
                }
            }
            else
            {
                ReturnToOriginalSlot();
            }
        }
        else
        {
            ReturnToOriginalSlot();
        }

        if (enchantSlotItems[0] is IEnchantable enchantableItem && enchantableItem.EnchantLevel <= 9)
        {
            if (enchantSlotItems[0] is EquipmentItem ei)
            {
                GameManager.Instance.EnchantManager.ToggleUI(true);
                GameManager.Instance.EnchantManager.SetRequireMentUI(ei);
            }
           
        }
        else
        {
            GameManager.Instance.EnchantManager.ToggleUI(false);
        }

    }

    private void ReturnToOriginalSlot()
    {
        if (beginDragSlot != null)
        {
            beginDragIconTransform.position = beginDragIconPoint;
            beginDragSlot.transform.SetSiblingIndex(beginDragSlotSiblingIndex);
        }
    }
}
