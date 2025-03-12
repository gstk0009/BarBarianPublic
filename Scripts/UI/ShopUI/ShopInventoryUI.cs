using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInventoryUI : MonoBehaviour
{
    [SerializeField] private ItemData[] sellingItems;
    [SerializeField] private TextMeshProUGUI playerGoldTxt;
    [SerializeField] private ShopItemSlotUI shopSlotUIPrefab;
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
            ShopItemSlotUI shopSlot = Instantiate(shopSlotUIPrefab);
            if (shopSlot == null)
            {
                shopSlot = shopSlot.gameObject.AddComponent<ShopItemSlotUI>();
            }


            shopSlot.SetItem(sellingItems[i]);

            shopSlotUIList.Add(shopSlot);
            shopSlot.gameObject.transform.SetParent(contentsAreaRT);
            shopSlot.gameObject.transform.localScale = Vector3.one;
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

            Vector2 pos = slot.gameObject.transform.localPosition;
            pos.x += xOffset;
            pos.y += yOffset;
            slot.gameObject.transform.localPosition = pos;
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

            Vector2 pos = slot.gameObject.transform.localPosition;
            pos.x -= xOffset;
            pos.y -= yOffset;
            slot.gameObject.transform.localPosition = pos;
        }
        equipmentSlotPosition.transform.SetAsLastSibling();
    }

    public void SetPlayerGold(int gold)
    {
        playerGoldTxt.text = gold.ToString();
    }
}
