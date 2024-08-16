using UnityEngine;

public class ItemSlotUIs : BaseItemSlotUI
{
    public bool isEquipmentRing = false;
    public bool isEquipmentWeapon = false;
    public bool isEquipmentHat = false;
    public bool isEquipmentArmor = false;
    public bool isEquipmentShield = false;

    private Color verylowquality = new Color(0.2f, 0.1f, 1.0f, 0.5f);
    private Color lowquality = new Color(1.0f, 1.0f, 0.1f, 0.5f);
    private Color middlequality = new Color(1.0f, 0.2f, 0.1f, 0.5f);
    private Color highquality = new Color(0.8f, 0.1f, 1.0f, 0.5f);

    public override bool SwapOrMoveIcon(BaseItemSlotUI other)
    {
        if (!(other is ItemSlotUIs otherSlot)) return false;

        if (otherSlot == null || otherSlot == this) return false;
        if (!this.IsAccessible || !otherSlot.IsAccessible) return false;

        if (otherSlot.isEquipmentArmor || otherSlot.isEquipmentHat || otherSlot.isEquipmentRing 
            || otherSlot.isEquipmentShield || otherSlot.isEquipmentWeapon)
        {
            SetEquipmentSlot(other, itemData);
        }

        //isNotEqualEquipmentSlot 대신 사용
        if (!IsRightSlot(otherSlot.itemData) || !otherSlot.IsRightSlot(this.itemData))
        {
            return false;
        }

        var tempSprite = iconImage.sprite;
        var tempItemData = itemData;
        Item slotItem = Player.Instance.inventory.GetItem(this.Index);

        if (HasItem)
        {
            if (isEquipmentArmor || isEquipmentHat || isEquipmentRing || isEquipmentShield || isEquipmentWeapon)
            {
                if (tempItemData is EquipmentItemData equipment)
                    Player.Instance.Animation.SetEmptyTexture(equipment.ItemLayer);
                SubtractEquipmentStat(slotItem);

                if (otherSlot.HasItem && otherSlot.itemData as EquipmentItemData)
                {
                    AddEquipmentStat(Player.Instance.inventory.GetItem(otherSlot.Index), this, true);
                }
                showGuideIconImage();
            }
        }

        // 드랍한 곳에 아이템이 있는 경우 : 교환
        if (other.HasItem)
            SetItem(other.iconImage.sprite);
        // 없는 경우 : 이동
        else
            RemoveItem();

        otherSlot.SetItem(tempSprite, tempItemData);

        return true;
    }

    public override void SetItem(Sprite itemSprite, ItemData data)
    {
        if (itemSprite != null)
        {
            iconImage.sprite = itemSprite;
            itemData = data;
            ShowIcon();
            showManaStoneType(data);
        }
        else
            RemoveItem();
    }

    private void showManaStoneType(ItemData data)
    {
        if (data is ManaStoneData manaStone)
        {
            if (manaStone != null)
            {
                switch (manaStone.StoneType)
                {
                    case ManaStoneType.E:
                        TypeBackgroundIamge.color = verylowquality;
                        break;
                    case ManaStoneType.D:
                        TypeBackgroundIamge.color = lowquality;
                        break;
                    case ManaStoneType.C:
                        TypeBackgroundIamge.color = middlequality;
                        break;
                    case ManaStoneType.B:
                        TypeBackgroundIamge.color = highquality;
                        break;
                }
                TypeBackgroundIamge.gameObject.SetActive(true);
            }
        }
    }

    public override void RemoveItem()
    {
        if(iconImage != null)
            iconImage.sprite = null;

        itemData = null; // 일반 itemData도 null로 설정
        itemAmount = 0;

        if(TypeBackgroundIamge != null)
            TypeBackgroundIamge.gameObject.SetActive(false);
        HideIcon();
        HideText();
    }

    private void SetEquipmentSlot(BaseItemSlotUI other, ItemData data)
    {
        if (!(other is ItemSlotUIs otherSlot)) return;
        if (!(data is EquipmentItemData equipmentData)) return;

        if ((otherSlot.isEquipmentArmor && equipmentData.Type == EquimentItemType.Armor)
            || (otherSlot.isEquipmentHat && equipmentData.Type == EquimentItemType.Hat)
            || (otherSlot.isEquipmentRing && equipmentData.Type == EquimentItemType.Ring)
            || (otherSlot.isEquipmentShield && equipmentData.Type == EquimentItemType.Sheld)
            || (otherSlot.isEquipmentWeapon && equipmentData.Type == EquimentItemType.Weapon))
        {
            hideGuideIconImage(otherSlot);
            Player.Instance.Animation.SwapTexture(equipmentData.ItemTexture, equipmentData.ItemLayer);
            Item currentslotItem = Player.Instance.inventory.GetItem(this.Index);
            AddEquipmentStat(currentslotItem, otherSlot);

            if (HasItem && otherSlot.HasItem)
            {
                SubtractEquipmentStat(Player.Instance.inventory.GetItem(otherSlot.Index), HasItem);
            }
        }
    }

    public override bool IsRightSlot(ItemData data)
    {
        // 일반 슬롯으로의 이동을 허용
        if (!isEquipmentArmor && !isEquipmentHat && !isEquipmentRing && !isEquipmentShield && !isEquipmentWeapon)
        {
            return true;
        }

        if (data == null) return true;

        if (data is EquipmentItemData equipData)
        {
            if (isEquipmentArmor && equipData.Type == EquimentItemType.Armor)
                return true;
            if (isEquipmentHat && equipData.Type == EquimentItemType.Hat)
                return true;
            if (isEquipmentRing && equipData.Type == EquimentItemType.Ring)
                return true;
            if (isEquipmentShield && equipData.Type == EquimentItemType.Sheld)
                return true;
            if (isEquipmentWeapon && equipData.Type == EquimentItemType.Weapon)
                return true;
        }

        return false;
    }

    private void AddEquipmentStat(Item slotItem, ItemSlotUIs other, bool isAlreadyEquip = false)
    {
        if ((isAlreadyEquip || other.isEquipmentArmor || other.isEquipmentHat) && slotItem is ArmorItem armor)
        {
            if (armor != null)
            {
                Player.Instance.playerStat.Def.AddMaxCurValue(armor.EnchantedStat());
                Player.Instance.SetPlayerStatUIDef();
            }
        }
        else if ((isAlreadyEquip ||other.isEquipmentShield || other.isEquipmentWeapon) && slotItem is WeaponItem weapon)
        {
            if (weapon != null)
            {
                Player.Instance.playerStat.STR.AddMaxCurValue(weapon.EnchantedStat());
                Player.Instance.SetPlayerStatUIStr();
            }
        }
    }

    private void SubtractEquipmentStat(Item slotItem, bool isAlreadyEquip = false)
    {
        if ((isEquipmentArmor || isEquipmentHat || isAlreadyEquip) && slotItem is ArmorItem armor)
        {
            if (armor != null)
            {
                Player.Instance.playerStat.Def.SubtractMaxCurValue(armor.EnchantedStat());
                Player.Instance.SetPlayerStatUIDef();
            }
        }
        else if ((isEquipmentShield || isEquipmentWeapon || isAlreadyEquip) && slotItem is WeaponItem weapon)
        {
            if (weapon != null)
            {
                Player.Instance.playerStat.STR.SubtractMaxCurValue(weapon.EnchantedStat());
                Player.Instance.SetPlayerStatUIStr();
            }
        }
    }
}
