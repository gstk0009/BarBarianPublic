using System.Collections;
using TMPro;
using UnityEngine;

public class EnchantMenuUI : MonoBehaviour
{
    public static EnchantMenuUI instance;

    [SerializeField] RectTransform slotArea;
    [SerializeField] float yOffset;
    [SerializeField] public InventoryUI inventoryUI;
    [SerializeField] private GameObject equipmentSlotPosition;
    public TextMeshProUGUI guideText;
    public EnchantItemSlot weaponSlot;
    public EnchantItemSlot materialSlot;
    public EnchantItemSlot resultSlot;

    string initText = "버튼을 눌러 장비 강화를 시도하세요!";
    bool isEnabled = false;
    WaitForSeconds wfs_1 = new WaitForSeconds(1);

    private void Start()
    {
        instance = this;
        SetupSlots();
    }

    private void SetupSlots()
    {
        weaponSlot.isWeaponSlot = true;
        materialSlot.isMaterialSlot = true;
        resultSlot.isResultSlot = true;
    }

    private void OnEnable()
    {
        guideText.text = initText;
        ShowInventorySlots();
    }

    private void OnDisable()
    {
        HideInventorySlots();
        ReturnItemsToInventory();
    }

    private void ReturnItemsToInventory()
    {
        if (weaponSlot.HasItem)
        {
            var emptySlot = inventoryUI.FindEmptySlot();
            if (emptySlot != null)
            {
                emptySlot.SetItem(weaponSlot.iconImage.sprite, weaponSlot.itemData);
                emptySlot.SetItemAmount(1);
                Player.Instance.inventory.AddItemDirectly(EnchantDragAndDrop.instance.enchantSlotItems[0] , emptySlot.Index);

                weaponSlot.RemoveItem();
                EnchantDragAndDrop.instance.enchantSlotItems[0] = null;
            }
        }

        if (materialSlot.HasItem)
        {
            var emptySlot = inventoryUI.FindEmptySlot();
            if (emptySlot != null)
            {
                emptySlot.SetItem(materialSlot.iconImage.sprite, materialSlot.itemData);

                Player.Instance.inventory.AddItemDirectly(EnchantDragAndDrop.instance.enchantSlotItems[1], 
                    emptySlot.Index, materialSlot.itemAmount);
                emptySlot.SetItemAmount(materialSlot.itemAmount);


                materialSlot.RemoveItem();
                EnchantDragAndDrop.instance.enchantSlotItems[1] = null;
            }
        }
    }

    public void ShowInventorySlots()
    {
        var slots = inventoryUI.GetLimitedSlotUIList(33);
        foreach (var slot in slots)
        {
            slot.transform.SetParent(slotArea);
            slot.transform.localScale = Vector3.one;
            if (slot.itemData != null && (EnchantMaterial.None == slot.itemData.enchantMaterial))
                slot.SetItemAccessibleState(false);
            RectTransform rt = slot.GetComponent<RectTransform>();
            Vector2 pos = rt.anchoredPosition;
            pos.y += yOffset;
            rt.anchoredPosition = pos;
        }
        isEnabled = true;
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
            if (isEnabled)
                pos.y -= yOffset;
            rt.anchoredPosition = pos;
        }
        equipmentSlotPosition.transform.SetAsLastSibling();
    }

    public IEnumerator MoveEnchantSlotItemToInventory(EnchantItemSlot slot, bool isResultItem = false)
    {
        yield return wfs_1;
        if (slot.HasItem)
        {
            //빈 슬롯을 찾아서 해당 슬롯의 인덱스 정보를 저장
            var emptySlot = inventoryUI.FindEmptySlot();
            int inventoryIndex = emptySlot.Index;

            if (emptySlot != null)
            {
                if(isResultItem)
                {
                    EquipmentItem wi = (EquipmentItem)EnchantDragAndDrop.instance.enchantSlotItems[2];
                    emptySlot.SetItem(wi.sprite, wi.Data);
                    emptySlot.SetItemAmount(1);
                    Player.Instance.inventory.AddItemDirectly(wi, inventoryIndex);
                    EnchantDragAndDrop.instance.enchantSlotItems[2] = null; // 할당 후 초기화 
                }
                else
                {
                    emptySlot.SetItem(slot.iconImage.sprite, slot.itemData);
                    int amount = slot.GetItemAmount();
                    inventoryUI.AddEnchantSlotItem(slot.itemData, emptySlot.Index, false, amount);
                }
                
                slot.RemoveItem();
            }
        }
    }

    public void EnchantItemBtn()
    {
        if (weaponSlot.HasItem && materialSlot.HasItem)
        {
            EquipmentItem _wi = (EquipmentItem)EnchantDragAndDrop.instance.enchantSlotItems[0];
            if(_wi.EnchantLevel > 9)
            {
                guideText.text = "장비가 이미 최대 강화 수치에 달했습니다!";
                return;
            }

            GameManager.Instance.EnchantManager.EnchantItem(weaponSlot, materialSlot, _wi);

            StartCoroutine(MoveEnchantSlotItemToInventory(resultSlot, true));
        }
        else
        {
            guideText.text = "비어있는 슬롯이 존재합니다!";
        }
    }
}
