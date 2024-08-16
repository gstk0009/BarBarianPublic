using Unity.VisualScripting;
using UnityEngine;

public class EquiptmentSlotUIs : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject[] equipmentItemSlots;
    [SerializeField] private GameObject itemSlotPosition;

    private RectTransform rectTrasnsform;

    static public bool isFirstOpen = true;
    private void Start()
    {
        rectTrasnsform = GetComponent<RectTransform>();
        inventoryUI = inventoryUI.GetComponent<InventoryUI>();
        SetEquipmentSlot();
        rectTrasnsform.SetAsLastSibling();
        itemSlotPosition.gameObject.SetActive(false);
    }
    
    private void SetEquipmentSlot()
    {
        for (int count = 0; count < equipmentItemSlots.Length; count++)
        {
            var slotUI = equipmentItemSlots[count].GetComponent<ItemSlotUIs>();
            int index = inventoryUI.GetSlotUIListCount();
            slotUI.SetSlotIndex(index);
            switch (count)
            {
                case 0:
                    slotUI.isEquipmentRing = true;
                    break;
                case 1:
                    slotUI.isEquipmentRing = true;
                    break;
                case 2:
                    slotUI.isEquipmentWeapon = true;
                    break;
                case 3:
                    slotUI.isEquipmentHat = true;
                    break;
                case 4:
                    slotUI.isEquipmentArmor = true;
                    break;
                case 5:
                    slotUI.isEquipmentShield = true;
                    break;
            }
            inventoryUI.AddSlotUIList(slotUI);
        }

        inventoryUI.InitInventory();
        if (DataManager.Instance.currentPlayer.inventoryItemDatas.Count > 0 && isFirstOpen)
        {
            InventorySaver.Instance.LoadInventory(DataManager.Instance.currentSaveDataSlot);
            isFirstOpen = false;
        }
    }
}
