using UnityEngine;

[CreateAssetMenu(fileName = "ItemWeapon", menuName = "ItemData/Weapon", order = 1)]
public class WeaponItemsData : EquipmentItemData
{
    public float WeaponStr => weaponStr;
    [SerializeField] private float weaponStr = 1;
    

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}
