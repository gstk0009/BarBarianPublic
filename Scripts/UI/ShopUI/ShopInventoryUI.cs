using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInventoryUI : MonoBehaviour
{
    [SerializeField] private ItemData[] sellingItems;
    [SerializeField] private TextMeshProUGUI playerGoldTxt;
    [SerializeField] private GameObject shopSlotUIPrefab;
    [SerializeField] private RectTransform contentsAreaRT;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private RectTransform slotArea;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private GameObject equipmentSlotPosition;

    private List<ShopItemSlotUI> shopSlotUIList = new List<ShopItemSlotUI>();

    private int createShopItemsCount;

    private void Start()
    {
        InitSlots();
        InitAccessibleSlot();
    }
    private void OnEnable()
    {
        playerGoldTxt.text = DataManager.Instance.currentPlayer.gold.ToString();

        if (inventoryUI == null)
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
        }
        ShowInventorySlots();
    }

    private void OnDisable()
    {
        HideInventorySlots();
    }

    private void InitSlots()
    {
        createShopItemsCount = sellingItems.Length;

        for (int i = 0; i < createShopItemsCount; i++)
        {
            // 각 슬롯을 개별적으로 인스턴스화
            GameObject shopSlot = Instantiate(shopSlotUIPrefab);
            ShopItemSlotUI shopItemSlot = shopSlot.GetComponent<ShopItemSlotUI>();
            if (shopItemSlot == null)
            {
                shopItemSlot = shopSlot.AddComponent<ShopItemSlotUI>();
            }

           
            shopItemSlot.SetItem(sellingItems[i]);

            shopSlot.transform.localScale = Vector3.one;
            shopSlotUIList.Add(shopItemSlot);
            RectTransform rt = shopSlot.GetComponent<RectTransform>();
            rt.SetParent(contentsAreaRT);
            rt.localScale = Vector3.one;
        }
    }

    private void InitAccessibleSlot()
    {
        foreach (var shopSlot in shopSlotUIList)
        {
            bool hasSingleUseItem = false;

            if (shopSlot.itemData is ShopItemData sd && sd.BuyType == Buyingitemtype.SingleUse)
            {
                hasSingleUseItem = Player.Instance.inventory.HasSingleUseItem(shopSlot.itemData.ID);
            }
            shopSlot.SetItemAccessibleState(!hasSingleUseItem);
        }
    }

    public void ShowInventorySlots()
    {
        var slots = inventoryUI.GetLimitedSlotUIList(33);
        foreach (var slot in slots)
        {
            slot.transform.SetParent(slotArea);
            slot.transform.localScale = Vector3.one;
            if (slot.itemData != null && (SellItem.Impossible == slot.itemData.Sell))
                slot.SetItemAccessibleState(false);

            RectTransform rt = slot.GetComponent<RectTransform>();
            Vector2 pos = rt.anchoredPosition;
            pos.x += xOffset;
            pos.y += yOffset;
            rt.anchoredPosition = pos;
        }
    }

    public void HideInventorySlots()
    {
        var slots = inventoryUI.GetLimitedSlotUIList(33);
        foreach (var slot in slots)
        {
            slot.transform.SetParent(inventoryUI.ContentArea);
            slot.transform.localScale = Vector3.one;
            if (slot.itemData != null)
                slot.SetItemAccessibleState(true);

            RectTransform rt = slot.GetComponent<RectTransform>();
            Vector2 pos = rt.anchoredPosition;
            pos.x -= xOffset;
            pos.y -= yOffset;
            rt.anchoredPosition = pos;
        }
        equipmentSlotPosition.transform.SetAsLastSibling();
    }

    public void SetPlayerGold(int gold)
    {
        playerGoldTxt.text = gold.ToString();
    }
}
