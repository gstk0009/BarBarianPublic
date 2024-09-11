using UnityEngine;

public class EnchantItemSlot : BaseItemSlotUI
{
    public bool isWeaponSlot = false;
    public bool isMaterialSlot = false;
    public bool isResultSlot = false;

    // 유효하지 않은 슬롯에 아이템 배치 시도를 감지하는 전역 변수
    public static bool invalidSlotAttempt = false;
    public static bool isEnchantSlot = false;

    public override bool SwapOrMoveIcon(BaseItemSlotUI other)
    {
        if (isResultSlot) return false;

        if (!(other is EnchantItemSlot otherSlot)) return false;

        if (otherSlot == null || otherSlot == this) return false;
        if (!this.IsAccessible || !otherSlot.IsAccessible) return false;

        if (IsRightSlot(otherSlot.itemData) && otherSlot.IsRightSlot(this.itemData))
        {
            var tempSprite = iconImage.sprite;
            var tempData = itemData;

            if (otherSlot.HasItem)
                SetItem(otherSlot.iconImage.sprite, otherSlot.itemData);
            else
                RemoveItem();

            otherSlot.SetItem(tempSprite, tempData);
            return true;
        }
        else
        {
            invalidSlotAttempt = true;
            return false;
        }
    }

    public override void SetItem(Sprite itemSprite, ItemData data)
    {
        invalidSlotAttempt = false;

        if (itemSprite != null && data != null)
        {
            if (IsRightSlot(data) || isResultSlot)
            {
                iconImage.sprite = itemSprite;
                itemData = data;

                // CountableItem의 경우 수량 설정
                if (data is CountableItemData countableData)
                {
                    CountableItem countableItem = countableData.CreateItem() as CountableItem;
                    SetItemAmount(countableItem.Amount);
                }
                if (!isEnchantSlot)
                {
                    ShowIcon();
                }
            }
            else
            {
                invalidSlotAttempt = true;
            }
        }
        else 
        {
            RemoveItem();
        }
    }



    public override bool IsRightSlot(ItemData data)
    {
        if (!isMaterialSlot && !isResultSlot && !isWeaponSlot)
        {
            return true;
        }

        if (data == null) return true;

       
        if (isWeaponSlot && data.enchantMaterial == EnchantMaterial.Enchantable)
            return true;
        else if (isMaterialSlot && data.enchantMaterial == EnchantMaterial.EnchantMaterial)
            return true;
        else if (isResultSlot)
            return false;

        return false;
    }
}
