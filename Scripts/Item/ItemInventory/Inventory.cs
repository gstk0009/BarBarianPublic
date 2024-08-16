using System;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public int itemId;
    public int enchantLevel;
    public int amount;
    public int itemsIndex;
    public float WeaponItemStr;
    public float ArmorItemDef;
    public float EnhanceRate;
}

public class Inventory : MonoBehaviour
{
    public int Capacity { get; private set; }

    [SerializeField, Range(11, 33)]
    private int initalCapacity = 11;

    [SerializeField, Range(11, 33)]
    private int maxCapacity = 33;

    private int equipmentCapacity = 39;

    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] InventoryUIDragAndDrop dragAndDrop;

    [SerializeField] private Item[] items;

    private bool isItemUseSucceed = false;

    private void Awake()
    {
        items = new Item[maxCapacity + 6];
        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<InventoryUI>();
        if (dragAndDrop == null)
            dragAndDrop = FindObjectOfType<InventoryUIDragAndDrop>();
        inventoryUI.SetInventoryReference(this);
        dragAndDrop.SetInventoryReference(this);
    }

    private void Start()
    {
        Capacity = DataManager.Instance.currentPlayer.inventoryCapacity;
        UpdateAccessibleStatesAll();

    }

    // Private Methods
    private bool IsValidIndex(int index)
    {
        return (index >= 0 && index < Capacity) || (index >= maxCapacity && index < equipmentCapacity);
    }

    public int FindEmptySlotIndex(int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
            if (items[i] == null)
                return i;
        return -1;
    }

    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = items[i];
            if (current == null) continue;

            if (current.Data == target && current is CountableItem ci)
            {
                if (!ci.IsMax)
                    return i;
            }
        }
        return -1;
    }

    public void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = items[index];

        if (item != null)
        {
            inventoryUI.SetItemIcon(index, item.Data.IconSprite, item.Data);

            if (item is CountableItem ci)
            {
                if (ci.IsEmpty)
                {
                    items[index] = null;
                    RemoveIcon();
                    return;
                }
                else
                {
                    inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
            else
            {
                inventoryUI.HideItemAmountText(index);
            }
        }
        else
        {
            RemoveIcon();
        }

        void RemoveIcon()
        {
            inventoryUI.RemoveItem(index);
            items[index] = null; 
            inventoryUI.HideItemAmountText(index);
        }
    }


    private void UpdateSlot(params int[] indices)
    {
        foreach (var i in indices)
        {
            UpdateSlot(i);
        }
    }

    private void UpdateAllSlot()
    {
        for (int i = 0; i < Capacity; i++)
        {
            UpdateSlot(i);
        }
    }

    // Public Methods

    public void UpdateAccessibleStatesAll()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity, maxCapacity);
    }

    public bool HasItem(int index)
    {
        return IsValidIndex(index) && items[index] != null;
    }

    public bool IsCountableItem(int index)
    {
        return HasItem(index) && items[index] is CountableItem;
    }

    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1;
        if (items[index] == null) return 0;

        CountableItem ci = items[index] as CountableItem;
        if (ci == null) return 1;

        return ci.Amount;
    }

    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (items[index] == null) return null;

        return items[index].Data;
    }

    public string GetItemName(int index)
    {
        if (!IsValidIndex(index) || items[index] == null) return "";

        return items[index].Data.Name;
    }

    public void Remove(int index)
    {
        items[index] = null;
        inventoryUI.RemoveItem(index);
    }
    
    public void Use(int index)
    {
        if (!IsValidIndex(index)) return;
        if (items[index] == null) return;

        if (items[index] is IUsableItem uItem)
        {
            isItemUseSucceed = uItem.Use();

            if (isItemUseSucceed)
            {
                UpdateSlot(index);

                if (items[index] is ShopItem shop)
                    Remove(index);

                isItemUseSucceed = false;
            }
        }
    }

    public int Add(ItemData itemData, int amount = 1, int index = -1, bool findNextCountable = true)
    {
        if (itemData is CountableItemData ciData)
        {
            while (amount > 0)
            {
                if (findNextCountable)
                {
                    if (index == -1)
                        index = FindCountableItemSlotIndex(ciData, index + 1);

                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    else
                    {
                        CountableItem ci = items[index] as CountableItem;
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                else
                {
                    if (index == -1)
                        index = FindEmptySlotIndex(index + 1);

                    if (index == -1)
                    {
                        break;
                    }
                    else
                    {
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        items[index] = ci;
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }
        else
        {
            if (amount == 1)
            {
                if (index == -1)
                    index = FindEmptySlotIndex();

                if (index != -1)
                {
                    items[index] = itemData.CreateItem();
                    amount = 0;

                    UpdateSlot(index);
                }
            }

            index = -1;
            for (; amount > 0; amount--)
            {
                index = FindEmptySlotIndex(index + 1);

                if (index == -1)
                    break;

                items[index] = itemData.CreateItem();

                UpdateSlot(index);
            }
        }

        return amount;
    }

    public void AddItemDirectly(Item item, int idx, int amount = 0)
    {
        items[idx] = item;
        if (items[idx] is CountableItem ci)
        {
            if(amount >= 1)
                ci.SetAmount(amount);
        }
    }

    public Item GetItem(int idx)
    {
        return items[idx];
    }

    public void Swap(int indexA, int indexB)
    {
        if (!IsValidIndex(indexA) || !IsValidIndex(indexB))
        {
            return;
        }

        Item itemA = items[indexA];
        Item itemB = items[indexB];

        if (itemA == null && itemB == null)
        {
            return;
        }

        if (itemA != null && itemA.Data == itemB?.Data && itemA is CountableItem ciA && itemB is CountableItem ciB)
        {
            int maxAmount = ciB.MaxAmount;
            int sum = ciA.Amount + ciB.Amount;

            if (sum <= maxAmount)
            {
                ciA.SetAmount(0);
                ciB.SetAmount(sum);
                items[indexA] = null;  // 교환 후 아이템 초기화
            }
            else
            {
                ciA.SetAmount(sum - maxAmount);
                ciB.SetAmount(maxAmount);
            }
        }
        else
        {
            items[indexA] = itemB;
            items[indexB] = itemA;

            // 빈 칸과의 swap의 경우 비워진 칸을 확실히 지우도록 구현 
            if (itemA == null)
            {
                ClearInventoryData(indexB);
            }
            else if (itemB == null)
            {
                ClearInventoryData(indexA);
            }
        }

        UpdateSlot(indexA, indexB);
    }


    public void SeparateAmount(int indexA, int indexB, int amount)
    {
        if (!IsValidIndex(indexA) || !IsValidIndex(indexB)) return;

        Item itemA = items[indexA];
        Item itemB = items[indexB];

        CountableItem ciA = itemA as CountableItem;

        if (ciA != null && itemB == null)
        {
            items[indexB] = ciA.SeperateAndClone(amount);

            UpdateSlot(indexA, indexB);
        }
    }

    public bool HasSingleUseItem(int ID)
    {
        foreach (var itemData in items)
        {
            if (itemData != null && itemData.Data.ID == ID) return true;
        }
        return false;
    }

    public bool AddCapacity(int value)
    {
        if ((Capacity + value) <= maxCapacity)
        {
            Capacity += value;
            UpdateAccessibleStatesAll();
            return true;
        }
        else
            return false;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                ClearInventoryData(i);
            }
        }
    }

    void ClearInventoryData(int index)
    {
        inventoryUI.RemoveItem(index);
        inventoryUI.ResetGuideIcon(index);
        
        items[index] = null;
    }

    public Item[] GetInventoryItems()
    {
        return items;
    }
}
