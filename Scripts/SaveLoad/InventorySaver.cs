using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Serialization<T>
{
    public T target;
    public Serialization(T target)
    {
        this.target = target;
    }
}

public class InventorySaver : MonoBehaviour
{
    public static InventorySaver Instance;
    private void Start()
    {
        Instance = this;
    }
    public List<InventoryItemData> GetInventoryData()
    {
        List<InventoryItemData> inventoryData = new List<InventoryItemData>();

        Item[] items = Player.Instance.inventory.GetInventoryItems();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                InventoryItemData itemData = new InventoryItemData
                {
                    itemId = items[i].Data.ID,
                    enchantLevel = items[i] is IEnchantable wi ? wi.EnchantLevel : 0,
                    EnhanceRate = items[i] is IEnchantable wi2 ? wi2.EnhanceRate : 0,
                    amount = items[i] is CountableItem countableItem ? countableItem.Amount : 1,
                    itemsIndex = i,
                    WeaponItemStr = items[i] is WeaponItem wi3 ? wi3.WeaponItemStr : default,
                    ArmorItemDef = items[i] is ArmorItem ai ? ai.ArmorItemDef : default,
                };
                inventoryData.Add(itemData);
            }
        }

        return inventoryData;
    }
    public void SaveInventory(int saveSlot)
    {
        List<InventoryItemData> inventoryData = GetInventoryData();
        string jsonData = JsonUtility.ToJson(new Serialization<List<InventoryItemData>>(inventoryData));
        PlayerPrefs.SetString("InventoryData_" + saveSlot, jsonData);
        PlayerPrefs.Save();
    }


    public void SetInventoryData(List<InventoryItemData> inventoryData)
    {
        Player.Instance.playerStat.InitializeStats();
        foreach (var data in inventoryData)
        {
            ItemData itemData = GameManager.Instance.itemManager.GetItemDataById(data.itemId);
            if (itemData != null)
            {
                Item item;
                if (itemData is CountableItemData countableItemData)
                {
                    item = countableItemData.CreateItem() as CountableItem;
                    (item as CountableItem).SetAmount(data.amount);
                }
                else if (itemData is WeaponItemsData weaponItemData)
                {
                    item = weaponItemData.CreateItem() as WeaponItem;
                    (item as WeaponItem).EnchantLevel = data.enchantLevel;
                    if (data.enchantLevel >= 1)
                    {
                        (item as WeaponItem).EnhanceRate = data.EnhanceRate;
                        (item as WeaponItem).ItemName = item.Data.Name + $" <color=#6CF6FF>(+{data.enchantLevel}강)</color>";
                    }
                    (item as WeaponItem).WeaponItemStr = data.WeaponItemStr;

                    if(data.itemsIndex >= 33)
                        Player.Instance.playerStat.STR.AddMaxCurValue((item as WeaponItem).EnchantedStat());
                    Player.Instance.Animation.SwapTexture(weaponItemData.ItemTexture, weaponItemData.ItemLayer);

                    Player.Instance.SetPlayerStatUIStr();
                }
                else if (itemData is ArmorItemsData armorItemsData)
                {
                    item = armorItemsData.CreateItem() as ArmorItem;
                    (item as ArmorItem).EnchantLevel = data.enchantLevel;
                    if (data.enchantLevel >= 1)
                    {
                        (item as ArmorItem).EnhanceRate = data.EnhanceRate;
                        (item as ArmorItem).ItemName = item.Data.Name + $" <color=#6CF6FF>(+{data.enchantLevel}강)</color>";
                    }
                    (item as ArmorItem).ArmorItemDef = data.ArmorItemDef;

                    if (data.itemsIndex >= 33)
                        Player.Instance.playerStat.Def.AddMaxCurValue((item as ArmorItem).EnchantedStat());
                    Player.Instance.Animation.SwapTexture(armorItemsData.ItemTexture, armorItemsData.ItemLayer);
                    Player.Instance.SetPlayerStatUIDef();
                }
                else
                {
                    item = itemData.CreateItem();
                }

                Player.Instance.inventory.AddItemDirectly(item, data.itemsIndex, data.amount);

                Player.Instance.inventory.UpdateSlot(data.itemsIndex);
            }
        }
    }

    public void LoadInventory(int saveSlot)
    {
        string jsonData = PlayerPrefs.GetString("InventoryData_" + saveSlot, "");
        if (!string.IsNullOrEmpty(jsonData))
        {
            var inventoryDataWrapper = JsonUtility.FromJson<Serialization<List<InventoryItemData>>>(jsonData);
            SetInventoryData(inventoryDataWrapper.target);
        }
    }
}