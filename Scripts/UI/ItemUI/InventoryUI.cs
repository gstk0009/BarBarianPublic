using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    [Header("Options")]
    [Range(0, 11)]
    [SerializeField] private int horizontalSlotCount = 11;   // 슬롯 가로 개수
    [Range(0, 10)]
    [SerializeField] private int verticalSlotCount = 3; // 슬롯 세로 개수
    [SerializeField] private float sloatMarginX = 10f;    // 한 슬롯의 상하좌우 여백
    [SerializeField] private float sloatMarginY = 8f;    // 한 슬롯의 상하좌우 여백
    [SerializeField] private float contentAreaPadding = 25f; // 인벤토리 영역의 내부 여백
    [Range(50, 140)]
    [SerializeField] private float slotSize = 110f;

    [Header("Connected Objects")]
    [SerializeField] private RectTransform contentAreaRT;   // 슬롯들이 위치할 영역
    [SerializeField] private GameObject slotUIPrefab;   // 슬롯의 프리팹
    [SerializeField] private TextMeshProUGUI playerGoldTxt;

    [SerializeField] private GameObject[] uiBackgrounds;

    private List<ItemSlotUIs> slotUIList = new List<ItemSlotUIs>();
    private Vector2 initPos = new Vector2(-705f, 240f);
    public RectTransform ContentArea => contentAreaRT; // 인벤토리 영역 참조 반환

    private void Awake()
    {
        InitSlots();
    }

    private void InitSlots()
    {
        // 슬롯 프리팹 설정
        slotUIPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        slotUIPrefab.TryGetComponent(out ItemSlotUIs itemSlot);
        if (itemSlot == null)
            slotUIPrefab.AddComponent<ItemSlotUIs>();

        slotUIPrefab.SetActive(true);

        Vector2 beginPos = initPos + new Vector2(contentAreaPadding + 25f, -(contentAreaPadding + 10f) ); // yOffset 적용
        Vector2 curPos = beginPos;

        slotUIList = new List<ItemSlotUIs>(verticalSlotCount * horizontalSlotCount);

        // 슬롯들 동적 생성
        for (int veritcal = 0; veritcal < verticalSlotCount; veritcal++)
        {
            for (int horizontal = 0; horizontal < horizontalSlotCount; horizontal++)
            {
                int slotIndex = (horizontalSlotCount * veritcal) + horizontal;

                var slotRT = CloneSlot();
                slotRT.pivot = new Vector2(0f, 0f);
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                var slotUI = slotRT.GetComponent<ItemSlotUIs>();
                slotUI.SetSlotIndex(slotIndex);
                slotUIList.Add(slotUI);

                if (slotIndex >= inventory.Capacity)
                    slotUI.SetSlotsAccessibleState(false);

                curPos.x += (sloatMarginX + slotSize);
            }

            curPos.x = beginPos.x;
            curPos.y -= (sloatMarginY + slotSize);
        }

        if (slotUIPrefab.scene.rootCount != 0)
            Destroy(slotUIPrefab);
    }

    RectTransform CloneSlot()
    {
        GameObject slotGo = Instantiate(slotUIPrefab);
        RectTransform rt = slotGo.GetComponent<RectTransform>();
        rt.SetParent(contentAreaRT, false);
        rt.localScale = Vector3.one; // 로컬 스케일 사이즈 초기화

        return rt;
    }

    public void InitInventory()
    {
        if (DataManager.Instance.setInventoryInit)
        {
            for (int i = 0; i < slotUIList.Count; i++)
            {
                InitInventoryItems(i);
            }

            DataManager.Instance.setInventoryInit = false;
        }
        else { }
    }

    public ItemSlotUIs FindEmptySlot()
    {
        foreach (var slot in slotUIList)
        {
            if (!slot.HasItem)
                return slot;
        }
        return null;
    }

    public void AddEnchantSlotItem(ItemData itemData, int idx = -1, bool findNextCountable = true, int amount = 1)
    {
        inventory.Add(itemData, amount, idx, findNextCountable);
    }

    // Public Methods

    // 인벤토리 참조 등록
    public void SetInventoryReference(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void SetItemIcon(int index, Sprite icon, ItemData data)
    {
        slotUIList[index].SetItem(icon, data);
    }

    public void SetItemAmountText(int index, int amount)
    {
        slotUIList[index].SetItemAmount(amount);
    }

    public void HideItemAmountText(int index)
    {
        slotUIList[index].SetItemAmount(1);
    }

    public void RemoveItem(int index)
    {
        slotUIList[index].RemoveItem();
    }
    public void ResetGuideIcon(int idx)
    {
        if (idx >= 33)
            slotUIList[idx].showGuideIconImage();
    }
    public void InitInventoryItems(int index)
    {
        slotUIList[index].RemoveItem();
    }

    // 접근 가능한 슬롯 범위 설정
    public void SetAccessibleSlotRange(int accessibleSlotCount, int maxLength)
    {
        for (int i = 0; i < maxLength; i++)
        {
            slotUIList[i].SetSlotsAccessibleState(i < accessibleSlotCount);
        }
    }

    public void AddSlotUIList(ItemSlotUIs addSlot)
    {
        slotUIList.Add(addSlot);
    }

    public int GetSlotUIListCount()
    {
        return slotUIList.Count;
    }
    public List<ItemSlotUIs> GetLimitedSlotUIList(int limit)
    {
        return slotUIList.GetRange(0, Mathf.Min(limit, slotUIList.Count));
    }

    public void SetInventoryPlayerGold()
    {
        playerGoldTxt.text = DataManager.Instance.currentPlayer.gold.ToString();
    }

    public void SetInventoryBackground(bool isActive)
    {
        uiBackgrounds[(int)InventoryBackgroundIndex.Item].SetActive(isActive);

        if (!isActive)
        {
            for (int index = (int)InventoryBackgroundIndex.elseStart; index <= (int)InventoryBackgroundIndex.elseEnd; index++)
            {
                uiBackgrounds[index].SetActive(false);
            }
        }
    }
}

public enum InventoryBackgroundIndex
{
    Item,
    elseStart,
    elseEnd = 4
}

